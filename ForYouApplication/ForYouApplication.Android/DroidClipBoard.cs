using Android.Content;
using Xamarin.Forms;
using ForYouApplication.Droid;

[assembly: Dependency(typeof(DroidClipBoard))]
namespace ForYouApplication.Droid
{
    public class DroidClipBoard : IClipBoard
    {
        public bool SaveClipBoard(string text)
        {
            var clipboardManager = (ClipboardManager)Forms.Context.GetSystemService(Context.ClipboardService);
            ClipData clip = ClipData.NewPlainText("", text);
            clipboardManager.PrimaryClip = clip;

            return true;
        }
    }
}