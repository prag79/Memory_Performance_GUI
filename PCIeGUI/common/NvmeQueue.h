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

#define SET_FEATURE_COMMAND  0x9
#define IDENTIFY_COMMAND     0x6
#define CREATE_IO_CQ_COMMAND 0x5
#define CREATE_IO_SQ_COMMAND 0x1
//#define WRITE_COMMAND        0x1
//#define READ_COMMAND         0x0
#define SQ_WIDTH              64
#define CQ_WIDTH              16



struct Dw0Common{
	uint8_t		opcode		;
	uint8_t		fuse	: 2	;
	uint8_t		rsvd	: 4	;
	uint8_t		PSDT	: 2 ;
	uint16_t	CID			;
};
typedef Dw0Common tDw0Common;

struct submissionQueue{
	tDw0Common	DW0;
	uint32_t	NSID;
	uint64_t	rsvd : 64;
	uint64_t	mptr;
	uint64_t	dptr1;
	uint64_t	dptr0;
	uint32_t	Dw10;
	uint32_t	Dw11;
	uint32_t	Dw12;
	uint32_t	Dw13;
	uint32_t	Dw14;
	uint32_t	Dw15;

};
typedef submissionQueue tSubQueue;

struct Dw2CompletionQueue{
	uint16_t	SQHPTR;
	uint16_t	SQID;
};
typedef Dw2CompletionQueue tDw2CompletionQueue;

struct Dw3CompletionQueue{
	uint16_t	CID;
	uint8_t		PT	: 1;
	uint16_t	SF	: 15;
};
typedef Dw3CompletionQueue tDw3CompletionQueue;

struct completionQueue{
	tDw0Common			Dw0;
	uint32_t			: 32;
	tDw2CompletionQueue	Dw2;
	tDw3CompletionQueue	Dw3;
};
typedef completionQueue tCompletionQueue;
//
////union Word{
////  uint16_t wholeWord;
////  uint8_t b0;
////  uint8_t b1;
////};
////
////union Dword{
////  uint32_t wholeDword;
////  Word Word0;
////  Word Word1;
////  uint8_t b0;
////  uint8_t b1;
////  uint8_t b2;
////  uint8_t b3;
////};
//
//struct Mptr{
//  Dword mptr0;
//  Dword mptr1;
//};
//
//struct Dptr{
//  Dword dptr0;
//  Dword dptr1;
//  Dword dptr2;
//  Dword dptr3;
//};
//struct Prp{
//	Dword dptr0;
//	Dword dptr1;
//};
//
//struct SubmissionQueue{
//private:
//  Dword dword0;
//  Dword nsid;
//  Dword res0;
//  Dword res1;
//  Mptr  mptr;
//  Dptr  dptr;
//  Dword dword10;
//  Dword dword11;
//  Dword dword12;
//  Dword dword13;
//  Dword dword14;
//  Dword dword15;
//public:
//  bool init(void);
//  uint8_t getOpcode(void);
//  bool setOpcode(uint8_t opcode);
//  uint16_t getCID(void);
//  bool setCID(uint16_t cid);
//  Dptr getDptr(void);
//  bool setDptr(Dptr dptr);
//  uint32_t getDword10(void);
//  bool setDword10(uint32_t dword);
//  uint32_t getDword11(void);
//  bool setDword11(uint32_t dword);
//  Prp getPrp1(void);
//  bool setPrp1(Prp prp);
//  bool getSV(void);
//  bool setSV(bool sv);
//  uint16_t getFID(void);
//  bool setFID(uint16_t fid);
//  uint16_t getNoSQ(void);
//  bool setNoSQ(uint16_t noSq);
//  uint16_t getNoCQ(void);
//  bool setNoCQ(uint16_t noCq);
//  uint64_t getLBA(void);
//  bool setLBA(uint64_t lba);
//  uint16_t getNoOfLBA(void);
//  bool setNoOfLBA(uint16_t noOfLba);
//  uint64_t getBaseAdd(void);
//  bool setBaseAdd(uint64_t add);
//  uint32_t getQSize(void);
//  bool setQSize(uint32_t qsize);
//  uint16_t getQID(void);
//  bool setQID(uint16_t qid);
//  uint16_t getCQID(void);
//  bool setCQID(uint16_t cqid);
//};
//
//struct CompletionQueue{
//private:
//  Dword dword0;
//  Dword dword1;
//  Dword dword2;
//  Dword dword3;
//public:
//  bool init(void);
//  uint16_t getSQID(void);
//  bool setSQID(uint16_t sqid);
//  uint16_t getSqhptr(void);
//  bool setSqhptr(uint16_t sqhptr);
//  uint8_t getSC(void);
//  bool setSC(uint8_t sc);
//  uint16_t getSF(void);
//  bool setSF(uint16_t sf);
//  bool getPhaseTag(void);
//  bool setPhaseTag(bool pt);
//  uint16_t getCID(void);
//  bool setCID(uint16_t cid);
//  uint32_t getDword0(void);
//  bool setDword0(uint32_t dword);
//  uint16_t getNoSQ(void);
//  bool setNoSQ(uint16_t noSq);
//  uint16_t getNoCQ(void);
//  bool setNoCQ(uint16_t noCq);
//};


struct CircularQueue{
private:
  uint32_t mHead;
  uint32_t mTail;
  uint32_t mQueueStart;
  uint32_t mQueueEnd;
  bool mIsSQ;
public:
  CircularQueue(bool isSQ, uint32_t baseAddress, uint32_t size);
  void setHead(uint32_t head);
  void setTail(uint32_t tail);
  bool isFull(void);
  bool isEmpty(void);
  uint32_t push(void);
  uint32_t pop(void);
  uint32_t getHead(void);
  uint32_t getTail(void);
};


