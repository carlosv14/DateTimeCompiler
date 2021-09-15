namespace DateTimeCompiler.Core.Statements
{
    public abstract class Statement: Node, ISemanticValidation
    {
        public abstract void ValidateSemantic();
        public abstract void Interpret();
    }
}