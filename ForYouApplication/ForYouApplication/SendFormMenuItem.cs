using System;

namespace ForYouApplication
{

    public class SendFormMenuItem
    {
        public SendFormMenuItem()
        {
            TargetType = typeof(SendFormDetail);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}