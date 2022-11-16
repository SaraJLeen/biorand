﻿using System.Diagnostics;
using System.IO;
using IntelOrca.Biohazard.Opcodes;

namespace IntelOrca.Biohazard
{

    [DebuggerDisplay("{Opcode}")]
    internal class DoorAotSeOpcode : OpcodeBase, IDoorAotSetOpcode
    {
        public override Opcode Opcode => Opcode.DoorAotSe;
        public override int Length => 32;

        public byte Id { get; set; }
        public byte SCE { get; set; }
        public byte SAT { get; set; }
        public byte Floor { get; set; }
        public byte Super { get; set; }
        public short X { get; set; }
        public short Y { get; set; }
        public short W { get; set; }
        public short H { get; set; }
        public short NextX { get; set; }
        public short NextY { get; set; }
        public short NextZ { get; set; }
        public short NextD { get; set; }
        public byte NextStage { get; set; }
        public byte NextRoom { get; set; }
        public byte NextCamera { get; set; }
        public byte NextFloor { get; set; }
        public byte Texture { get; set; }
        public byte Animation { get; set; }
        public byte Sound { get; set; }
        public byte KeyId { get; set; }
        public byte KeyType { get; set; }
        public byte Free { get; set; }

        public RdtId Target
        {
            get => new RdtId(NextStage, NextRoom);
            set
            {
                NextStage = (byte)value.Stage;
                NextRoom = (byte)value.Room;
            }
        }

        public static DoorAotSeOpcode Read(BinaryReader br, int offset)
        {
            return new DoorAotSeOpcode()
            {
                Offset = offset,
                Id = br.ReadByte(),
                SCE = br.ReadByte(),
                SAT = br.ReadByte(),
                Floor = br.ReadByte(),
                Super = br.ReadByte(),
                X = br.ReadInt16(),
                Y = br.ReadInt16(),
                W = br.ReadInt16(),
                H = br.ReadInt16(),
                NextX = br.ReadInt16(),
                NextY = br.ReadInt16(),
                NextZ = br.ReadInt16(),
                NextD = br.ReadInt16(),
                NextStage = br.ReadByte(),
                NextRoom = br.ReadByte(),
                NextCamera = br.ReadByte(),
                NextFloor = br.ReadByte(),
                Texture = br.ReadByte(),
                Animation = br.ReadByte(),
                Sound = br.ReadByte(),
                KeyId = br.ReadByte(),
                KeyType = br.ReadByte(),
                Free = br.ReadByte()
            };
        }

        public override void Write(BinaryWriter bw)
        {
            bw.Write((byte)Opcode);
            bw.Write(Id);
            bw.Write(SCE);
            bw.Write(SAT);
            bw.Write(Floor);
            bw.Write(Super);
            bw.Write(X);
            bw.Write(Y);
            bw.Write(W);
            bw.Write(H);
            bw.Write(NextX);
            bw.Write(NextY);
            bw.Write(NextZ);
            bw.Write(NextD);
            bw.Write(NextStage);
            bw.Write(NextRoom);
            bw.Write(NextCamera);
            bw.Write(NextFloor);
            bw.Write(Texture);
            bw.Write(Animation);
            bw.Write(Sound);
            bw.Write(KeyId);
            bw.Write(KeyType);
            bw.Write(Free);
        }
    }
}
