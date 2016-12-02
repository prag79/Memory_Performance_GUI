/*******************************************************************
 * File : tlp.h
 *
 * Copyright(C) crossbar-inc. 
 * 
 * ALL RIGHTS RESERVED.
 *
 * Description: This file contains all the common format for TLP
 * Related Specifications: 
 *
 ********************************************************************/
#pragma once
#include <cstdint>

struct Dw0MemoryTLP{
	uint16_t	length			: 10;
	uint16_t	dw0UnusedField	: 14;
	uint8_t		type			: 5;
	uint8_t		format			: 2;
	bool		dw0UnusedBit	: 1;

};
typedef Dw0MemoryTLP tDw0MemoryTLP;

struct Dw1MemoryTLP{
	uint8_t		byteEnable	: 8;
	uint8_t		tag			: 8;
	uint16_t	requesterID	: 16;
};
typedef Dw1MemoryTLP tDw1MemoryTLP;

//struct Dw2MemoryTLP {
//	uint8_t	 rsvd		: 2;
//	uint32_t address	: 30;
//};
//typedef Dw2MemoryTLP tDw2MemoryTLP;

struct Dw1CompletionTLP{
	uint16_t	byteCount		: 12;
	uint8_t		dw1UnusedField	: 4;
	uint16_t	completerID;
};
typedef Dw1CompletionTLP tDw1CompletionTLP;

struct Dw2CompletionTLP{
	uint8_t		lowerAddress	: 7;
	uint8_t		rsvd			: 1;
	uint8_t		tag;
	uint16_t	requesterID;
};
typedef Dw2CompletionTLP tDw2CompletionTLP;

struct MemoryWriteReqTLP{
	tDw0MemoryTLP	Dw0MemWriteReqTLP;
	tDw1MemoryTLP	Dw1MemWriteReqTLP;
	uint32_t		address;
	uint8_t*		data;
};
typedef MemoryWriteReqTLP tMemoryWriteReqTLP;

struct MemoryReadReqTLP{
	tDw0MemoryTLP	Dw0MemReadReqTLP;
	tDw1MemoryTLP	Dw1MemReadReqTLP;
	uint32_t		address;
};
typedef MemoryReadReqTLP tMemoryReadReqTLP;

struct CompletionTLP{
	tDw0MemoryTLP			Dw0CompletionTLP;
	tDw1CompletionTLP		Dw1CompletionTLP;
	tDw2CompletionTLP		Dw2CompletionTLP;
	uint8_t*				data;
};
typedef CompletionTLP tCompletionTLP;

struct ConfigWriteReq{
	tDw0MemoryTLP		Dw0CfgWriteReqTLP;
	tDw1MemoryTLP		Dw1CfgWriteReqTLP;
	uint32_t			address;
	uint64_t			data;
};
typedef ConfigWriteReq tConfigWriteReq;

struct ConfigReadReq{
	tDw0MemoryTLP		Dw0CfgReadReqTLP;
	tDw1MemoryTLP		Dw1CfgReadReqTLP;
	uint32_t			address;
};
typedef ConfigReadReq tConfigReadReq;