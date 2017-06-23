namespace Where.Engine
{
    public abstract class GameObject
    {
        public abstract void OnUpdate();

        public abstract bool Died { get; }
    }
}