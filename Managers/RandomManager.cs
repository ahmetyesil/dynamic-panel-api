using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managers
{
    public class RandomManager : ManagerBase<RandomManager>
    {
        private Lazy<Random> random = new Lazy<Random>();
        private Random Random
        {
            get
            {
                var next = random.Value.Next();
                Guid guid = Guid.NewGuid();
                int seed = (int)unchecked(((next ^ 2) << 2) + next * next * (~next | 3) + guid.GetHashCode() + DateTime.UtcNow.Ticks ^ 2);
                return new Random(Math.Abs(seed));
            }
        }

        public double CreatePercentage()
        {
            return Random.NextDouble() * 100;
        }

        public string CreateSecurityCode(int count)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = Random;
            return new string(Enumerable.Repeat(chars, count).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public string CreateHash()
        {
            var keyword = CreateSecurityCode(10) + CreatePercentage();
            return HashManager.Instance.Md5(keyword);
        }
    }
}
