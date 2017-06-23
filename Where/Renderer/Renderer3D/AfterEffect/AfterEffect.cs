namespace Where.Renderer.Renderer3D.AfterEffect
{
    public interface IAfterEffect
    {
        void SetDrawState();

        void ResetDrawState();

        int DownSample { get; }
        int Live { get; }
    }
}