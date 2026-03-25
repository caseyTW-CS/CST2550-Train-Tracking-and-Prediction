namespace TrainProject
{
    public class TrainSpeed
    {
        //PROPERTIES
        public string trainType { get; private set; }

        public int averageSpeed { get; private set; }

        public int topSpeed { get; private set; }

        //METHODS
        public TrainSpeed(string type, int avgSpeed, int maxSpeed)
        {
            trainType = type;
            averageSpeed = avgSpeed;
            topSpeed = maxSpeed;
        }

        public double estimateJourneyTime(double distanceMiles)
        {
            return (distanceMiles / averageSpeed) * 60;
        }
    }
}