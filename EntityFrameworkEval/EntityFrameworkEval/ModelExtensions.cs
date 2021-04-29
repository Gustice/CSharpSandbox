using RamTestDb;

namespace EntityFrameworkEval
{
    public static class ModelExtensions
    {
        public static string ListEntry(this MyModel model)
        {
            return $"    # {model.Id}: {model.SomeEntry}/{model.OtherEntry}";
        }
    }
}
