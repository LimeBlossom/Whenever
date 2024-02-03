namespace WheneverAbstractions._Project.WheneverAbstractions.Commands
{
    public record AddWheneverCommand : IWorldCommand
    {
        public Whenever whenever;
    
        public AddWheneverCommand(Whenever whenever)
        {
            this.whenever = whenever;
        }

        public void ApplyCommand(ICommandableWorld world)
        {
            world.AddWhenever(whenever);
        }
    }
}