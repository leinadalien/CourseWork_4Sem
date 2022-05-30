using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork
{
    public class Leaf
    {
        private const int MIN_LEAF_SIZE = 36;
        private const int MAX_LEAF_SIZE = 52;
        public IntRect Bounds { get; private set; }
        public Leaf? LeftChild { get; private set; }
        public Leaf? RightChild { get; private set; }
        public IntRect RoomBounds { get; private set; }
        public Leaf(IntRect bounds)
        {
            Bounds = bounds;
        }
        public void Split(Random random)
        {
            if (LeftChild != null || RightChild != null)
            {
                return;
            }
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
                Vector2i roomSize = new(random.Next(16, Bounds.Width - 3), random.Next(16, Bounds.Height - 3));
                Vector2i roomLocalPosition = new(random.Next(1, Bounds.Width - roomSize.X - 1), random.Next(1, Bounds.Height - roomSize.Y - 1));
                RoomBounds = new(roomLocalPosition.X + Bounds.Left, roomLocalPosition.Y + Bounds.Top, roomSize.X, roomSize.Y);
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
            if (LeftChild.Bounds.Width > MAX_LEAF_SIZE || LeftChild.Bounds.Height > MAX_LEAF_SIZE || random.NextDouble() >= 0.25)
            {
                LeftChild.Split(random);
            }
            if (RightChild.Bounds.Width > MAX_LEAF_SIZE || RightChild.Bounds.Height > MAX_LEAF_SIZE || random.NextDouble() >= 0.25)
            {
                RightChild.Split(random);
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
    }
}
