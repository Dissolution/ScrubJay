// ReSharper disable InconsistentNaming

#pragma warning disable S4663, CA1028, CA1711, CA1720

namespace ScrubJay.Reflection.Decompilation;

/// <summary>
/// 
/// </summary>
/// <seealso cref="MetadataTokenType"/>
/// <seealso href="https://learn.microsoft.com/en-us/previous-versions/dotnet/netframework-4.0/ms404456(v=vs.100)"/>
[PublicAPI]
public enum MetadataTokenType : byte
{
    Module = /*                 */ 0b_00000000,
    TypeRef = /*                */ 0b_00000001,
    TypeDef = /*                */ 0b_00000010,
    FieldPtr = /*               */ 0b_00000011,
    FieldDef = /*               */ 0b_00000100,
    MethodPtr = /*              */ 0b_00000101,
    MethodDef = /*              */ 0b_00000110,
    ParamPtr = /*               */ 0b_00000111,
    ParamDef = /*               */ 0b_00001000,
    InterfaceImpl = /*          */ 0b_00001001,
    MemberRef = /*              */ 0b_00001010,
    Constant = /*               */ 0b_00001011,
    CustomAttribute = /*        */ 0b_00001100,
    FieldMarshal = /*           */ 0b_00001101,
    Permission = /*             */ 0b_00001110,
    DeclSecurity = Permission,
    ClassLayout = /*            */ 0b_00001111,
    FieldLayout = /*            */ 0b_00010000,
    Signature = /*              */ 0b_00010001,
    StandAloneSig = Signature,
    EventMap = /*               */ 0b_00010010,
    EventPtr = /*               */ 0b_00010011,
    Event = /*                  */ 0b_00010100,
    XEvent = Event,
    PropertyMap = /*            */ 0b_00010101,
    PropertyPtr = /*            */ 0b_00010110,
    Property = /*               */ 0b_00010111,
    XProperty = Property,
    MethodSemantics = /*        */ 0b_00011000,
    MethodImpl = /*             */ 0b_00011001,
    ModuleRef = /*              */ 0b_00011010,
    TypeSpec = /*               */ 0b_00011011,
    ImplMap = /*                */ 0b_00011100,
    FieldRVA = /*               */ 0b_00011101,
    EditAndContinueLog = /*     */ 0b_00011110,
    EditAndContinueMap = /*     */ 0b_00011111,
    Assembly = /*               */ 0b_00100000,
    XAssembly = Assembly,
    AssemblyProcessor = /*      */ 0b_00100001,
    AssemblyOS = /*             */ 0b_00100010,
    AssemblyRef = /*            */ 0b_00100011,
    AssemblyRefProcessor = /*   */ 0b_00100100,
    AssemblyRefOS = /*          */ 0b_00100101,
    File = /*                   */ 0b_00100110,
    ExportedType = /*           */ 0b_00100111,
    ManifestResource = /*       */ 0b_00101000,
    NestedClass = /*            */ 0b_00101001,
    GenericParam = /*           */ 0b_00101010,
    MethodSpec = /*             */ 0b_00101011,
    GenericMethod = MethodSpec,
    GenericParamConstraint = /* */ 0b_00101100,
    GenericConstraint = GenericParamConstraint,
    // [47..63) unused
    MaxTable = /*               */ 0b_00111111,

    String = /*                 */ 0b_01110000, // special, lower 3 bytes represent an offset to the string's starting location in the metadata string pool
    Name = /*                   */ 0b_01110001,
    BaseType = /*               */ 0b_01110010,

    Invalid = /*                */ 0b_01111111,
}