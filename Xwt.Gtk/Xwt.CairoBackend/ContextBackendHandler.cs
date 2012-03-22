// 
// ContextBackendHandler.cs
//  
// Author:
//       Lluis Sanchez <lluis@xamarin.com>
// 
// Copyright (c) 2011 Xamarin Inc
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using Xwt.Backends;
using Xwt.Engine;
using Xwt.Drawing;

namespace Xwt.CairoBackend
{
	class CairoContextBackend
	{
		public double GlobalAlpha = 1;
		public Cairo.Context Context;
		public Cairo.Surface TempSurface;
	}
	
	public class ContextBackendHandler: IContextBackendHandler
	{
		public ContextBackendHandler ()
		{
		}

		#region IContextBackendHandler implementation
		
		public void Save (object backend)
		{
			CairoContextBackend gc = (CairoContextBackend)backend;
			gc.Context.Save ();
		}
		
		public void Restore (object backend)
		{
			CairoContextBackend gc = (CairoContextBackend)backend;
			gc.Context.Restore ();
		}
		
		public void SetGlobalAlpha (object backend, double alpha)
		{
			CairoContextBackend gc = (CairoContextBackend) backend;
			gc.GlobalAlpha = alpha;
		}

		public void Arc (object backend, double xc, double yc, double radius, double angle1, double angle2)
		{
			CairoContextBackend gc = (CairoContextBackend) backend;
			gc.Context.Arc (xc, yc, radius, (angle1 * System.Math.PI) / 180, (angle2 * System.Math.PI) / 180);
		}

		public void Clip (object backend)
		{
			Cairo.Context ctx = ((CairoContextBackend) backend).Context;
			ctx.Clip ();
		}

		public void ClipPreserve (object backend)
		{
			Cairo.Context ctx = ((CairoContextBackend) backend).Context;
			ctx.ClipPreserve ();
		}

		public void ResetClip (object backend)
		{
			Cairo.Context ctx = ((CairoContextBackend) backend).Context;
			ctx.ResetClip ();
		}

		public void ClosePath (object backend)
		{
			Cairo.Context ctx = ((CairoContextBackend) backend).Context;
			ctx.ClosePath ();
		}

		public void CurveTo (object backend, double x1, double y1, double x2, double y2, double x3, double y3)
		{
			CairoContextBackend gc = (CairoContextBackend) backend;
			gc.Context.CurveTo (x1, y1, x2, y2, x3, y3);
		}

		public void Fill (object backend)
		{
			var gtkc = (CairoContextBackend) backend;
			Cairo.Context ctx = gtkc.Context;
			if (gtkc.GlobalAlpha == 1)
				ctx.Fill ();
			else {
				ctx.PushGroup ();
				ctx.Fill ();
				ctx.PopGroupToSource ();
				ctx.PaintWithAlpha (gtkc.GlobalAlpha);
			}
		}

		public void FillPreserve (object backend)
		{
			Cairo.Context ctx = ((CairoContextBackend) backend).Context;
			ctx.FillPreserve ();
		}

		public void LineTo (object backend, double x, double y)
		{
			CairoContextBackend gc = (CairoContextBackend) backend;
			gc.Context.LineTo (x, y);
		}

		public void MoveTo (object backend, double x, double y)
		{
			CairoContextBackend gc = (CairoContextBackend) backend;
			gc.Context.MoveTo (x, y);
		}

		public void NewPath (object backend)
		{
			Cairo.Context ctx = ((CairoContextBackend) backend).Context;
			ctx.NewPath ();
		}

		public void Rectangle (object backend, double x, double y, double width, double height)
		{
			CairoContextBackend gc = (CairoContextBackend) backend;
			gc.Context.Rectangle (x, y, width, height);
		}

		public void RelCurveTo (object backend, double dx1, double dy1, double dx2, double dy2, double dx3, double dy3)
		{
			Cairo.Context ctx = ((CairoContextBackend) backend).Context;
			ctx.RelCurveTo (dx1, dy1, dx2, dy2, dx3, dy3);
		}

		public void RelLineTo (object backend, double dx, double dy)
		{
			Cairo.Context ctx = ((CairoContextBackend) backend).Context;
			ctx.RelLineTo (dx, dy);
		}

		public void RelMoveTo (object backend, double dx, double dy)
		{
			Cairo.Context ctx = ((CairoContextBackend) backend).Context;
			ctx.RelMoveTo (dx, dy);
		}

		public void Stroke (object backend)
		{
			Cairo.Context ctx = ((CairoContextBackend) backend).Context;
			ctx.Stroke ();
		}

		public void StrokePreserve (object backend)
		{
			Cairo.Context ctx = ((CairoContextBackend) backend).Context;
			ctx.StrokePreserve ();
		}

		public void SetColor (object backend, Xwt.Drawing.Color color)
		{
			var gtkContext = (CairoContextBackend) backend;
			gtkContext.Context.Color = new Cairo.Color (color.Red, color.Green, color.Blue, color.Alpha * gtkContext.GlobalAlpha);
		}
		
		public void SetLineWidth (object backend, double width)
		{
			Cairo.Context ctx = ((CairoContextBackend) backend).Context;
			ctx.LineWidth = width;
		}
		
		public void SetLineDash (object backend, double offset, params double[] pattern)
		{
			Cairo.Context ctx = ((CairoContextBackend) backend).Context;
			ctx.SetDash (pattern, offset);
		}
		
		public void SetPattern (object backend, object p)
		{
			Cairo.Context ctx = ((CairoContextBackend)backend).Context;
			if (p != null)
				ctx.Pattern = (Cairo.Pattern) p;
			else
				ctx.Pattern = null;
		}
		
		public void SetFont (object backend, Font font)
		{
		}
		
		public void DrawTextLayout (object backend, TextLayout layout, double x, double y)
		{
			Pango.Layout pl = (Pango.Layout) WidgetRegistry.GetBackend (layout);
			CairoContextBackend ctx = (CairoContextBackend) backend;
			ctx.Context.MoveTo (x, y);
			Pango.CairoHelper.ShowLayout (ctx.Context, pl);
		}
		
		public void DrawImage (object backend, object img, double x, double y, double alpha)
		{
			Gdk.Pixbuf pb = (Gdk.Pixbuf)img;
			CairoContextBackend ctx = (CairoContextBackend)backend;
			Gdk.CairoHelper.SetSourcePixbuf (ctx.Context, pb, x, y);
			alpha = alpha * ctx.GlobalAlpha;
			if (alpha == 1)
				ctx.Context.Paint ();
			else
				ctx.Context.PaintWithAlpha (alpha);
		}
		
		public void DrawImage (object backend, object img, double x, double y, double width, double height, double alpha)
		{
			Gdk.Pixbuf pb = (Gdk.Pixbuf)img;
			CairoContextBackend ctx = (CairoContextBackend)backend;
			ctx.Context.Save ();
			double sx = ((double) width) / pb.Width;
			double sy = ((double) height) / pb.Height;
			ctx.Context.Translate (x, y);
			ctx.Context.Scale (sx, sy);
			Gdk.CairoHelper.SetSourcePixbuf (ctx.Context, pb, 0, 0);
			alpha = alpha * ctx.GlobalAlpha;
			if (alpha == 1)
				ctx.Context.Paint ();
			else
				ctx.Context.PaintWithAlpha (alpha);
			ctx.Context.Restore ();
		}
		
		public void ResetTransform (object backend)
		{
			CairoContextBackend gc = (CairoContextBackend)backend;
			gc.Context.IdentityMatrix();
		}

        public void Rotate (object backend, double angle)
		{
			CairoContextBackend gc = (CairoContextBackend)backend;
			gc.Context.Rotate ((angle * System.Math.PI) / 180);
		}
		
		public void Translate (object backend, double tx, double ty)
		{
			CairoContextBackend gc = (CairoContextBackend)backend;
			gc.Context.Translate (tx, ty);
		}
		
		public void Dispose (object backend)
		{
			var ctx = (CairoContextBackend) backend;
			IDisposable d = (IDisposable) ctx.Context;
			d.Dispose ();
            d = (IDisposable)ctx.TempSurface;
			if (d != null)
				d.Dispose ();
		}
		#endregion
	}
}
