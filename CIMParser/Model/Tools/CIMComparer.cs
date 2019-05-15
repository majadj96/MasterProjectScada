
namespace CIM.Model
{
    /// <summary>
    /// CIMComparer class contains static fields for sorting objects.
    /// </summary>
    public static class CIMComparer
    {
        public static ModelElementComparer<ProfileElement> ProfileElementComparer = new ModelElementComparer<ProfileElement>();
        public static ModelElementComparer<ProfileElementStereotype> ProfileElementStereotypeComparer = new ModelElementComparer<ProfileElementStereotype>();
        public static ModelElementComparer<CIMObject> CIMObjectComparer = new ModelElementComparer<CIMObject>();        
    }
}
