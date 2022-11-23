﻿using System.Diagnostics;
using System.IO;
using IntelOrca.Biohazard.Opcodes;
using IntelOrca.Biohazard.Script;

namespace IntelOrca.Biohazard
{
    [DebuggerDisplay("cmp")]
    internal class CmpOpcode : OpcodeBase
    {
        public byte Unknown1 { get; set; }
        public byte Flag { get; set; }
        public byte Operator { get; set; }
        public short Value { get; set; }

        public static CmpOpcode Read(BinaryReader br, int offset)
        {
            var opcode = br.ReadByte();
            if ((OpcodeV1)opcode == OpcodeV1.Cmp6)
            {
                return new CmpOpcode()
                {
                    Offset = offset,
                    Length = 4,

                    Opcode = opcode,
                    Flag = br.ReadByte(),
                    Operator = br.ReadByte(),
                    Value = br.ReadByte()
                };
            }
            else
            {
                return new CmpOpcode()
                {
                    Offset = offset,
                    Length = 6,

                    Opcode = opcode,
                    Unknown1 = br.ReadByte(),
                    Flag = br.ReadByte(),
                    Operator = br.ReadByte(),
                    Value = br.ReadInt16()
                };
            }
        }

        public override void Write(BinaryWriter bw)
        {
            bw.Write(Opcode);
            bw.Write(Unknown1);
            bw.Write(Flag);
            bw.Write(Operator);
            bw.Write(Value);
        }
    }
}
