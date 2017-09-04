using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using CoreGraphics;
using UIKit;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

using ForYouApplication;
using ForYouApplication.iOS;
using System.Drawing;

[assembly:ExportRenderer(typeof(MyBoxView), typeof(MyBoxViewRenderer))]
namespace ForYouApplication.iOS
{
    public class MyBoxViewRenderer : BoxRenderer
    {
        public override void Draw(CGRect rect)
        {
            var view = (MyBoxView)Element;
            using (var context = UIGraphics.GetCurrentContext())
            {   
                var shadowSize = view.ShadowSize;
                var blur = shadowSize;
                var radius = view.Radius;

                context.SetFillColor(view.MainColor.ToCGColor());
                var bounds = Bounds.Inset(shadowSize * 2, shadowSize * 2);
                context.AddPath(CGPath.FromRoundedRect(bounds, radius, radius));
                context.SetShadow(new SizeF(shadowSize, shadowSize), blur);
                context.DrawPath(CGPathDrawingMode.Fill);
            }
        }
    }
}