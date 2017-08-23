using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForYouApplication
{

    public class StartPageMenuItem
    {
        public StartPageMenuItem()
        {
            TargetType = typeof(StartPageDetail);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}