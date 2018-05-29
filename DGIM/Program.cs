using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGIM
{
    public class Program
    {
        public static int maxCount = 2;
        public static int length, total, last, timeNow;
        public static List<Bucket> buckets;
        public static int maxSize = 1;

        static void Main(string[] args)
        {
            length = int.Parse(Console.ReadLine());
            total = 0;
            last = 0;
            timeNow = 0;
            string readLine;
            string exitLine = "";
            buckets = new List<Bucket>();

            while ((readLine = Console.ReadLine()) != null)
            {
                if (!readLine.StartsWith("q"))
                {
                    Update(readLine);
                }
                else
                {
                    var bit = int.Parse(readLine.Substring(2));
                    exitLine += DefineBit(bit) + "\n";
                }
            }

            Console.WriteLine(exitLine);
        }

        public static string DefineBit(int bit)
        {
            int cnt = -1;
            int criticalTime = timeNow - bit;
            for (int i = 0; i < buckets.Count; i++)
            {
                var bucket = buckets[i];
                if (bucket.time <= criticalTime)
                {
                    break;
                }
                cnt = i;
            }
            int sum = 0;

            for (int i = 0; i < buckets.Count; i++)
            {
                var bucket = buckets[i];
                if (i < cnt)
                {
                    sum += bucket.size;
                }
                else
                {
                    sum += bucket.size/2;
                    break;
                }
            }

            return sum.ToString();
        }

        public static void ModifyBuckets(Bucket bucket)
        {
            buckets.Insert(0, bucket);
            for (int i = 0; i <= maxSize; i++)
            {
                int lastSize = 0;
                int cnt = 0;
                for (int j = 0; j < buckets.Count && buckets[j].size <= i; j++)
                {
                    if (buckets[j].size == i)
                    {
                        cnt++;
                        lastSize = j;
                    }
                }

                if (cnt > maxCount)
                {
                    var newBucket = buckets[lastSize].ConnectBuckets(buckets[lastSize - 1]);
                    buckets[lastSize - 1] = newBucket;
                    buckets.RemoveAt(lastSize);
                }
            }
            last = buckets[buckets.Count - 1].size;
        }

        public static void Update(string bits)
        {
            for (int i = 0; i < bits.Length; i++)
            {
                var bit = bits[i];
                var timeEnd = ++timeNow - length;
                if (buckets.Any() && buckets[buckets.Count - 1].time <= timeEnd)
                {
                    var toDelete = buckets[buckets.Count - 1];
                    buckets.RemoveAt(buckets.Count - 1);

                    total -= toDelete.size;
                    last = !buckets.Any() ? 0 : buckets[buckets.Count - 1].size;
                }

                if (bit == '1')
                {
                    total++;
                    var bucket = new Bucket(1);
                    bucket.time = timeNow;
                    ModifyBuckets(bucket);
                }
            }
        }

        public class Bucket
        {
            public int size;
            public int time;

            public Bucket()
            {
                size = 0;
                time = 0;
            }

            public Bucket(int size)
            {
                this.size = size;
                time = 0;
            }

            public Bucket ConnectBuckets(Bucket bucket)
            {
                if (bucket.size == this.size)
                {
                    Bucket newBucket = new Bucket(bucket.size * 2);
                    newBucket.time = Math.Max(this.time, bucket.time);

                    if (newBucket.size > maxSize)
                        maxSize = newBucket.size;

                    return newBucket;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
