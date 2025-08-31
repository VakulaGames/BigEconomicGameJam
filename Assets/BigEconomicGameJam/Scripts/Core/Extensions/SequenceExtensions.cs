using DG.Tweening;

namespace CORE
{
    public static class SequenceExtensions
    {
        public static Sequence MakeSequence(this Sequence sequence)
        {
            sequence.SafeKill();
            sequence = DOTween.Sequence();
            return sequence;
        }

        public static void SafeKill(this Sequence sequence)
        {
            sequence?.Kill();
            sequence = null;
        }
    }
}