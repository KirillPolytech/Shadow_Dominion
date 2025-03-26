namespace Shadow_Dominion.Main
{
    public class Singleton<T>
    {
        public static T Instance { get; protected set; }
    }
}