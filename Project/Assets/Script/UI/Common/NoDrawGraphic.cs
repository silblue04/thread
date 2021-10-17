using UnityEngine.UI;

public class NoDrawGraphic : Graphic
{
    public override void SetMaterialDirty( ) { }
    public override void SetVerticesDirty( ) { }
    protected override void OnPopulateMesh( VertexHelper vh )
    {
        vh.Clear( );
    }
}
