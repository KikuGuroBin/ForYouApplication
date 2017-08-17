namespace ForYouApplication
{
    public class ManipulationDeltaRoutedEventArgs
    {
        public object OriginalSource { get; set; }

        /* アクションの種類 */
        public int Action { get; set; }
        
        /* 移動距離差分 */
        public Translation Translation { get; set; }

        public ManipulationDeltaRoutedEventArgs(object source, double deltaX, double deltaY, int action)
        {
            OriginalSource = source;
            Action = action;
            Translation = new Translation()
            {
                X = deltaX,
                Y = deltaY
            };
        }
    }
}