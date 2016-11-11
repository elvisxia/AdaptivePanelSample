using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AdaptivePanelSample.Panels
{
    public class AdaptivePanel:Panel
    {
        private int rows;
        private int cols;
        
        protected override Size MeasureOverride(Size availableSize)
        {
            int i = 0;
            int childrenCount = Children.Count;
            //calculate the row count /column count of the panel
            rows = (int)Math.Ceiling(Math.Sqrt((double)childrenCount));
            cols = rows;
            double elementWidth,elementHeight = 0;
            elementWidth = availableSize.Width / cols;
            //for stackpanel height is infinity, take it into consideration
            if (!double.IsInfinity(availableSize.Height))
            {
                elementHeight = availableSize.Height / rows;
            }
            else
            {
                elementHeight = elementWidth;
            }

            foreach (FrameworkElement child in Children)
            {
                //mesure the children
                child.Measure(new Size(elementWidth, elementHeight));
            }
            
            return new Size(availableSize.Width,double.IsInfinity(availableSize.Height)?availableSize.Width:availableSize.Height);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {

            // Get total number of children
            int count = Children.Count;
            int index = 0;
            UIElement child=null;
            //arrange the elements in the panel
            for(int i=0;i<rows;i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (index >= count)
                    {
                        break;
                    }
                    child = Children[index];
                    double boxLength = finalSize.Width / cols;
                    //colNumber=j, rowNumber=i
                    double x = finalSize.Width / cols * j;
                    double y = finalSize.Height / rows * i;
                    //if the element is bigger than the box then use the normal anchorPoint otherwise let it be in the middle of the box.
                    double elementX=x, elementY=y;
                    if (child.DesiredSize.Width < boxLength)
                    {
                        elementX = x + (boxLength - child.DesiredSize.Width) / 2;
                    }
                    if (child.DesiredSize.Height < boxLength)
                    {
                        elementY = y + (boxLength - child.DesiredSize.Height) / 2;
                    }
                    
                    Point anchorPoint = new Point(elementX, elementY);
                    Children[index].Arrange(new Rect(anchorPoint,child.DesiredSize));
                    index++;
                }
            }

            
            return finalSize;
        }


    }
}
