using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork
{
    public class Map
    {
        private const int MIN_LEAF_SIZE = 36;
        private const int MAX_LEAF_SIZE = 52;
        public IntRect Bounds { get; private set; }
        public Map? LeftChild { get; private set; }
        public Map? RightChild { get; private set; }
        public IntRect RoomBounds { get; private set; }
        public Map(IntRect bounds)
        {
            Bounds = bounds;
        }
        public void Split(Random random)
        {


            bool horisontalSplit = random.NextDouble() >= 0.5;
            if (Bounds.Width * 1.0f / Bounds.Height >= 1.25)
            {
                horisontalSplit = false;
            }
            else if (Bounds.Height * 1.0f / Bounds.Width >= 1.25)
            {
                horisontalSplit = true;
            }
            int maxLeafSize = (horisontalSplit ? Bounds.Height : Bounds.Width) - MIN_LEAF_SIZE;
            if (maxLeafSize < MIN_LEAF_SIZE)
            {

                return;
            }
            int leftLeafSize = random.Next(MIN_LEAF_SIZE, maxLeafSize);
            if (horisontalSplit)
            {
                LeftChild = new(new(Bounds.Left, Bounds.Top, Bounds.Width, leftLeafSize));
                RightChild = new(new(Bounds.Left, Bounds.Top + leftLeafSize, Bounds.Width, Bounds.Height - leftLeafSize));
            }
            else
            {
                LeftChild = new(new(Bounds.Left, Bounds.Top, leftLeafSize, Bounds.Height));
                RightChild = new(new(Bounds.Left + leftLeafSize, Bounds.Top, Bounds.Width - leftLeafSize, Bounds.Height));
            }
            Map temp = LeftChild;
            for (int i = 0; i < 2; i++)
            {
                if (temp.Bounds.Width > MAX_LEAF_SIZE || temp.Bounds.Height > MAX_LEAF_SIZE || random.NextDouble() >= 0.25)
                {
                    temp.Split(random);
                }
                if (temp.LeftChild == null && temp.RightChild == null)
                {
                    Vector2i roomSize = new(random.Next(16, temp.Bounds.Width - 3), random.Next(16, temp.Bounds.Height - 3));
                    Vector2i roomLocalPosition = new(random.Next(1, temp.Bounds.Width - roomSize.X - 1), random.Next(1, temp.Bounds.Height - roomSize.Y - 1));
                    temp.RoomBounds = new(roomLocalPosition.X + temp.Bounds.Left, roomLocalPosition.Y + temp.Bounds.Top, roomSize.X, roomSize.Y);
                }
                temp = RightChild;
            }
        }
        public List<IntRect> GetRooms()
        {
            List<IntRect> result = new();
            if (LeftChild != null)
            {
                result.AddRange(LeftChild.GetRooms());
            }
            if (RightChild != null)
            {
                result.AddRange(RightChild.GetRooms());
            }
            if (LeftChild == null && RightChild == null)
            {
                result.Add(RoomBounds);
            }
            return result;
        } 
        public static List<IntRect> CreateTransition(IntRect start, IntRect end, Random random)
        {
			List<IntRect> result = new();
			int transitionWidth = 8;
			Vector2i startPoint = new(start.Left + random.Next(1, start.Width - transitionWidth - 1),start.Top + random.Next(1, start.Height - transitionWidth - 1));
			Vector2i endPoint = new(end.Left + random.Next(1, end.Width - transitionWidth - 1), end.Top + random.Next(1, end.Height - transitionWidth - 1));
			int width = endPoint.X - startPoint.X;
			var height = endPoint.Y - startPoint.Y;

			if (width < 0)
			{
				if (height < 0)
				{
					if (random.NextDouble() < 0.5)
					{
						result.Add(new IntRect(endPoint.X, startPoint.Y, -width, transitionWidth));
						result.Add(new IntRect(endPoint.X, endPoint.Y, transitionWidth, -height));
					}
					else
					{
						result.Add(new IntRect(endPoint.X, endPoint.Y, -width, transitionWidth));
						result.Add(new IntRect(startPoint.X, endPoint.Y, transitionWidth, -height));
					}
				}
				else if (height > 0)
				{
					if (random.NextDouble() < 0.5)
					{
						result.Add(new IntRect(endPoint.X, startPoint.Y, -width, transitionWidth));
						result.Add(new IntRect(endPoint.X, startPoint.Y, transitionWidth, height));
					}
					else
					{
						result.Add(new IntRect(endPoint.X, endPoint.Y, -width, transitionWidth));
						result.Add(new IntRect(startPoint.X, startPoint.Y, transitionWidth, height));
					}
				}
				else
				{
					result.Add(new IntRect(endPoint.X, endPoint.Y, -width, transitionWidth));
				}
			}
			else if (width > 0)
			{
				if (height < 0)
				{
					if (random.NextDouble() < 0.5)
					{
						result.Add(new IntRect(startPoint.X, endPoint.Y, width, transitionWidth));
						result.Add(new IntRect(startPoint.X, endPoint.Y, transitionWidth, -height));
					}
					else
					{
						result.Add(new IntRect(startPoint.X, startPoint.Y, width, transitionWidth));
						result.Add(new IntRect(endPoint.X, endPoint.Y, transitionWidth, -height));
					}
				}
				else if (height > 0)
				{
					if (random.NextDouble() < 0.5)
					{
						result.Add(new IntRect(startPoint.X, startPoint.Y, width, transitionWidth));
						result.Add(new IntRect(endPoint.X, startPoint.Y, transitionWidth, height));
					}
					else
					{
						result.Add(new IntRect(startPoint.X, endPoint.Y, width, transitionWidth));
						result.Add(new IntRect(startPoint.X, startPoint.Y, transitionWidth, height));
					}
				}
				else
				{
					result.Add(new IntRect(startPoint.X, startPoint.Y, width, transitionWidth));
				}
			}
			else
			{
				if (height < 0)
				{
					result.Add(new IntRect(endPoint.X, endPoint.X, transitionWidth, -height));
				}
				else if (height > 0)
				{
					result.Add(new IntRect(startPoint.X, startPoint.Y, transitionWidth, height));
				}
			}
			return result;
		}
    }
}
