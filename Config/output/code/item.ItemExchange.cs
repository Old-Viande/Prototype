
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Luban;
using SimpleJSON;


namespace cfg.item
{
public sealed partial class ItemExchange : Luban.BeanBase
{
    public ItemExchange(JSONNode _buf) 
    {
        { if(!_buf["id"].IsNumber) { throw new SerializationException(); }  Id = _buf["id"]; }
        { if(!_buf["num"].IsNumber) { throw new SerializationException(); }  Num = _buf["num"]; }
    }

    public static ItemExchange DeserializeItemExchange(JSONNode _buf)
    {
        return new item.ItemExchange(_buf);
    }

    /// <summary>
    /// 道具id
    /// </summary>
    public readonly int Id;
    /// <summary>
    /// 道具数量
    /// </summary>
    public readonly int Num;
   
    public const int __ID__ = 1814660465;
    public override int GetTypeId() => __ID__;

    public  void ResolveRef(Tables tables)
    {
        
        
    }

    public override string ToString()
    {
        return "{ "
        + "id:" + Id + ","
        + "num:" + Num + ","
        + "}";
    }
}

}
