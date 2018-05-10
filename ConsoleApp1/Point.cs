namespace MyNotSoLittlePryczkoptron
{
    public class Point
    {
        public double XCoordinate;
        public double YCoordinate;

        public Point(double XCoord, double YCoord)
        {
            this.XCoordinate = XCoord;
            this.YCoordinate = YCoord;
        }

        Point Sum(Point FirstPoint, Point LatterPoint)
        {
            return new Point(FirstPoint.XCoordinate + LatterPoint.XCoordinate,  FirstPoint.YCoordinate + LatterPoint.YCoordinate);
        }
    }
}
