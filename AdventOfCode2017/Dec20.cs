using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode2017
{
    public static class Dec20
    {
        public static void Run(string path = null)
        {
            Console.WriteLine(Utilities.LineSeparator);
            Console.WriteLine("December 20th - Particle Swarm -");
            Console.WriteLine("Part1");
            Part1(Path.Combine(path,"dec20.txt"), 161);

            Console.WriteLine();
            Console.WriteLine("Part2");
            Part2(Path.Combine(path, "dec20.txt"), 438);
        }
        
        private class Particle
        {
            public int id;
            public bool destroyed = false;
            public long px, py, pz;
            public long vx, vy, vz;
            public long ax, ay, az;
            
            public Particle(int id, string[] split)
            {
                SetParticle(id, split);
            }

            public void SetParticle(int id, string[] split)
            {
                this.id = id;

                px = long.Parse(split[0]);
                py = long.Parse(split[1]);
                pz = long.Parse(split[2]);

                vx = long.Parse(split[3]);
                vy = long.Parse(split[4]);
                vz = long.Parse(split[5]);

                ax = long.Parse(split[6]);
                ay = long.Parse(split[7]);
                az = long.Parse(split[8]);
            }

            public long DistanceToZero()
            {
                return Math.Abs(px) + Math.Abs(py) + Math.Abs(pz);
            }

            public long VelocitySum
            {
                get
                {
                    return Math.Abs(vx) + Math.Abs(vy) + Math.Abs(vz);
                }
            }

            public long AccellerationSum
            {
                get
                {
                    return Math.Abs(ax) + Math.Abs(ay) + Math.Abs(az);
                }
            }

            internal void Increment()
            {
                vx += ax;
                vy += ay;
                vz += az;

                px += vx;
                py += vy;
                pz += vz;
            }

            internal bool SameLocation(Particle other)
            {
                if (px == other.px && py == other.py && pz == other.pz)
                    return true;

                return false;
            }
        }
 
        /// <summary>
        /// Find the particle with the lowest accelleration. 
        /// </summary>
        public static Result Part1(string filename, int? expected = null)
        {
            var input = Utilities.LoadStrings(filename);

            List<Particle> particles = new List<Particle>();

            int idcounter = 0;
            foreach(var line in input)
            {
                var str = line.Replace("p=<", "");
                str = str.Replace(" v=<", "");
                str = str.Replace(" a=<", "");
                str = str.Replace(">", "");
                var split = str.Split(',');
                particles.Add(new Particle(idcounter++, split));
            }

            int[] found = new int[particles.Count];


            List<long> vel = new List<long>();
            List<long> acc = new List<long>();
            var minv = long.MaxValue;
            var mina = long.MaxValue;
            int indexv = -1;
            int indexa = -1;
            foreach (var particle in particles)
            {
                vel.Add(particle.VelocitySum);
                if (particle.VelocitySum < minv)
                {
                    minv = particle.VelocitySum;
                    indexv = particle.id;
                }

                acc.Add(particle.AccellerationSum);
                if (particle.AccellerationSum < mina)
                {
                    mina = particle.AccellerationSum;
                    indexa = particle.id;
                }
            }

            var result = particles.OrderBy(a => a.AccellerationSum).ThenBy(a => a.VelocitySum);

            Utilities.WriteInputFile(filename);
            return Utilities.WriteOutput(result.First().id, expected);
        }


        /// <summary>
        /// Count particles that didn't collide.
        /// </summary>
        public static Result Part2(string filename, int? expected = null)
        {
            var input = Utilities.LoadStrings(filename);

            List<Particle> particles = new List<Particle>();

            int idcounter = 0;
            foreach (var line in input)
            {
                var str = line.Replace("p=<", "");
                str = str.Replace(" v=<", "");
                str = str.Replace(" a=<", "");
                str = str.Replace(">", "");
                var split = str.Split(',');
                particles.Add(new Particle(idcounter++, split));
            }

            int[] found = new int[particles.Count];

            for(int i = 0; i< 100; i++)
            {
                foreach (var particle in particles)
                {
                    if (!particle.destroyed)
                    {
                        foreach (var other in particles)
                        {
                            if(particle != other && particle.SameLocation(other))
                            {
                                particle.destroyed = true;
                                other.destroyed = true;
                            }
                        }
                    }
                }

                foreach (var particle in particles)
                {
                    if (!particle.destroyed)
                        particle.Increment();
                }

            }

            int count = 0;
            foreach(var particle in particles)
            {
                if (!particle.destroyed)
                    count++;
            }

            Utilities.WriteInputFile(filename);
            return Utilities.WriteOutput(count, expected);
        }
    }
}
