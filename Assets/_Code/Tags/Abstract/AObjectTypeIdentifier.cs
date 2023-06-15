namespace Assets.Tags.Abstract
{
    public abstract class AObjectTypeIdentifier : ATag
    {
        protected const string IdentifierAssetMenuBaseName = AssetMenuBaseName + "Identifier/";

        public override int GetHashCode() => Id.GetHashCode();
        public override string ToString() => $"[Identifier --> {name}]";
    }
}
