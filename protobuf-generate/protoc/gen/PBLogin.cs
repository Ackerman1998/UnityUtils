// <auto-generated>
//   This file was generated by a tool; you should avoid making direct changes.
//   Consider using 'partial classes' to extend these types
//   Input: PBLogin.proto
// </auto-generated>

#region Designer generated code
#pragma warning disable CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
namespace PBLogin
{

    [global::ProtoBuf.ProtoContract()]
    public partial class TcpRequestConnect : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"token", IsRequired = true)]
        public string Token { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class TcpRequestLogin : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"account", IsRequired = true)]
        public string Account { get; set; }

        [global::ProtoBuf.ProtoMember(2, Name = @"password", IsRequired = true)]
        public string Password { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class TcpResponseLogin : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"result", IsRequired = true)]
        public bool Result { get; set; }

        [global::ProtoBuf.ProtoMember(2, Name = @"uid", IsRequired = true)]
        public int Uid { get; set; }

        [global::ProtoBuf.ProtoMember(3, IsRequired = true)]
        public int udpPort { get; set; }

    }

}

#pragma warning restore CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
#endregion
