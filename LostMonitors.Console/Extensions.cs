using LostMonitors.Core;

namespace LostMonitors.Console
{
    public static class Extensions
    {
        public static string ToFriendly(this Card card)
        {
            return string.Format("[{0}, {1}]", card.Destination, card.Value);
        }
        public static string ToConcise(this Card card)
        {
            return string.Format("[{0}, {1}]", (int)card.Destination, (int)card.Value);
        }
    }
}