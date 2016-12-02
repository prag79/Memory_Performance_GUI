/*******************************************************************
 * File : NvmeQueue.h
 *
 * Copyright(C) crossbar-inc. 
 * 
 * ALL RIGHTS RESERVED.
 *
 * Description: This file contains all the common header for NVME Queue
 * Related Specifications: 
 *
 ********************************************************************/
#pragma once
#include <cstdint>

struct Dw0MemoryTLP{
	uint16_t length			: 10;
	uint8_t					: 2;
	uint8_t attribute		: 2;
	uint8_t EP				: 1;
	uint8_t TD				: 1;
	uint8_t					: 4;
	uint8_t trafficClass	: 3;
	uint8_t					: 1;
	uint8_t type			: 5;
	uint8_t format			: 2;
	uint8_t					: 1;

};
typedef Dw0MemoryTLP tDw0MemoryTLP;

struct Dw1MemoryTLP{
	uint8_t  firstByteEnable	: 4;
	uint8_t  lastByteEnable		: 4;
	uint8_t  tag				: 4;
	uint16_t requesterID;
};
typedef Dw1MemoryTLP tDw1MemoryTLP;

struct Dw2MemoryTLP {
	uint8_t				: 2;
	uint32_t address	: 30;
};
typedef Dw2MemoryTLP tDw2MemoryTLP;

struct Dw1CompletionTLP{
	uint16_t byteCount	: 12;
	uint8_t  bcm		: 1;
	uint8_t	 status		: 3;
	uint16_t completerID;
};
typedef Dw1CompletionTLP tDw1CompletionTLP;

struct Dw2CompletionTLP{
	uint8_t  lowerAddress	: 7;
	uint8_t					: 1;
	uint8_t  tag;
	uint16_t requesterID;
};
typedef Dw2CompletionTLP tDw2CompletionTLP;

struct MemWriteReq{
	tDw0MemoryTLP Dw0MemoryWriteReqTLP;
	tDw1MemoryTLP Dw1MemoryWriteReqTLP;
	tDw2MemoryTLP Dw2MemoryWriteReqTLP;
	uint32_t	  data;
};
typedef MemWriteReq tMemWriteReq;

struct MemReadReq{
	tDw0MemoryTLP Dw0MemoryWriteReqTLP;
	tDw1MemoryTLP Dw1MemoryWriteReqTLP;
	tDw2MemoryTLP Dw2MemoryWriteReqTLP;
};
typedef MemReadReq tMemReadReq;

struct ConfigWriteReq{
	tDw0MemoryTLP Dw0MemoryWriteReqTLP;
	tDw1MemoryTLP Dw1MemoryWriteReqTLP;
	tDw2MemoryTLP Dw2MemoryWriteReqTLP;
	uint64_t	  data;
};
typedef ConfigWriteReq tConfigWriteReq;

struct ConfigReadReq{
	tDw0MemoryTLP Dw0MemoryWriteReqTLP;
	tDw1MemoryTLP Dw1MemoryWriteReqTLP;
	tDw2MemoryTLP Dw2MemoryWriteReqTLP;
};
typedef ConfigReadReq tConfigReadReq;

struct CompletionTLP{
	tDw0MemoryTLP		Dw0CompletionTLP;
	tDw1CompletionTLP	Dw1CompletionTLP;
	tDw2CompletionTLP	Dw2CompletionTLP;
	uint8_t*			data;
};
typedef CompletionTLP tCompletionTLP;
