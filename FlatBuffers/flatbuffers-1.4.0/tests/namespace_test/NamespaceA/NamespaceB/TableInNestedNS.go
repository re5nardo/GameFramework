// automatically generated by the FlatBuffers compiler, do not modify

package NamespaceB

import (
	flatbuffers "github.com/google/flatbuffers/go"
)

type TableInNestedNS struct {
	_tab flatbuffers.Table
}

func GetRootAsTableInNestedNS(buf []byte, offset flatbuffers.UOffsetT) *TableInNestedNS {
	n := flatbuffers.GetUOffsetT(buf[offset:])
	x := &TableInNestedNS{}
	x.Init(buf, n+offset)
	return x
}

func (rcv *TableInNestedNS) Init(buf []byte, i flatbuffers.UOffsetT) {
	rcv._tab.Bytes = buf
	rcv._tab.Pos = i
}

func (rcv *TableInNestedNS) Foo() int32 {
	o := flatbuffers.UOffsetT(rcv._tab.Offset(4))
	if o != 0 {
		return rcv._tab.GetInt32(o + rcv._tab.Pos)
	}
	return 0
}

func (rcv *TableInNestedNS) MutateFoo(n int32) bool {
	return rcv._tab.MutateInt32Slot(4, n)
}

func TableInNestedNSStart(builder *flatbuffers.Builder) {
	builder.StartObject(1)
}
func TableInNestedNSAddFoo(builder *flatbuffers.Builder, foo int32) {
	builder.PrependInt32Slot(0, foo, 0)
}
func TableInNestedNSEnd(builder *flatbuffers.Builder) flatbuffers.UOffsetT {
	return builder.EndObject()
}
