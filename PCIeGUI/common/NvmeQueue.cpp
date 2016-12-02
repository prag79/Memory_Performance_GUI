/*******************************************************************
 * File : NvmeQueue.cpp
 *
 * Copyright(C) crossbar-inc. 
 * 
 * ALL RIGHTS RESERVED.
 *
 * Description: This file contains definitions for functionality related to NVME Queues
 * Related Specifications: 
 *
 ********************************************************************/
#include "NvmeQueue.h"

//bool CompletionQueue::init(void){
//  dword0.wholeDword = 0x00000000;
//  dword1.wholeDword = 0x00000000;
//  dword2.wholeDword = 0x00000000;
//  dword3.wholeDword = 0x00000000;
//  return true;
//}
//
//uint16_t CompletionQueue::getSQID(void){
//  return (dword2.Word1.wholeWord);
//}
//
//bool CompletionQueue::setSQID(uint16_t sqid){
//  dword2.Word1.wholeWord = sqid;
//  return true;
//}
//
//uint16_t CompletionQueue::getSqhptr(void){
//  return (dword2.Word0.wholeWord);
//}
//
//bool CompletionQueue::setSqhptr(uint16_t sqhptr){
//  dword2.Word0.wholeWord = sqhptr;
//  return true;
//}
//
//uint8_t CompletionQueue::getSC(void){
//  uint16_t sf = getSF();
//  return( (sf & 0x00FF));
//}
//
//bool CompletionQueue::setSC(uint8_t sc){
//  uint16_t sf = getSF();
//  sf = (sf & 0xFF00) | sc;
//  return(setSF(sf));
//}
//
//uint16_t CompletionQueue::getSF(void){
//  return ((dword3.Word1.wholeWord >> 1) & 0x7FFF);
//}
//
//bool CompletionQueue::setSF(uint16_t sf){
//  dword3.Word1.wholeWord = (dword3.Word1.wholeWord & 0x1) | ((sf << 1) & 0xFFFE);
//  return true;
//}
//
//bool CompletionQueue::getPhaseTag(void){
//  return (dword3.Word1.b0 & 0x1);
//}
//
//bool CompletionQueue::setPhaseTag(bool pt){
//  dword3.Word1.b0 = (dword3.Word1.b0 & 0xFE) | pt;  //set Phase tag to 0 and then ORed with receive phase tag
//  return true;
//}
//
//uint16_t CompletionQueue::getCID(void){
//  return (dword3.Word0.wholeWord);
//}
//
//bool CompletionQueue::setCID(uint16_t cid){ 
//  dword3.Word0.wholeWord = cid;
//  return true;
//}
//
//uint32_t CompletionQueue::getDword0(void){
//  return (dword0.wholeDword);
//}
//
//bool CompletionQueue::setDword0(uint32_t dword){
//  dword0.wholeDword = dword;
//  return true;
//}
//
//uint16_t CompletionQueue::getNoSQ(void){
//  return (dword0.Word0.wholeWord);
//}
//
//bool CompletionQueue::setNoSQ(uint16_t noSq){
//  dword0.Word0.wholeWord = noSq;
//  return true;
//}
//
//uint16_t CompletionQueue::getNoCQ(void){
//  return(dword0.Word1.wholeWord);
//}
//
//bool CompletionQueue::setNoCQ(uint16_t noCq){
//  dword0.Word1.wholeWord = noCq;
//  return true;
//}
//
//bool SubmissionQueue::init(void){
//  dword0.wholeDword     = 0x00000000;
//  nsid.wholeDword       = 0x00000000;
//  res0.wholeDword       = 0x00000000;
//  res1.wholeDword       = 0x00000000;
//  mptr.mptr0.wholeDword = 0x00000000;
//  mptr.mptr1.wholeDword = 0x00000000;
//  dptr.dptr0.wholeDword = 0x00000000;
//  dptr.dptr1.wholeDword = 0x00000000;
//  dptr.dptr2.wholeDword = 0x00000000; 
//  dptr.dptr3.wholeDword = 0x00000000;
//  dword10.wholeDword    = 0x00000000;
//  dword11.wholeDword    = 0x00000000;
//  dword12.wholeDword    = 0x00000000;
//  dword13.wholeDword    = 0x00000000;
//  dword14.wholeDword    = 0x00000000;
//  dword15.wholeDword    = 0x00000000;
//  return true;
//}
//
//uint8_t SubmissionQueue::getOpcode(void){
//  return (dword0.b0);
//}
//
//bool SubmissionQueue::setOpcode(uint8_t opcode){
//  dword0.b0 = opcode;
//  return true;
//}
//
//uint16_t SubmissionQueue::getCID(void){
//  return (dword0.Word1.wholeWord);
//}
//
//bool SubmissionQueue::setCID(uint16_t cid){
//  dword0.Word1.wholeWord = cid;
//  return true;
//}
//
//Dptr SubmissionQueue::getDptr(void){
//  return dptr;
//}
//
//bool SubmissionQueue::setDptr(Dptr dptr){
//  dptr = dptr;
//  return true;
//}
//
//uint32_t SubmissionQueue::getDword10(void){
//  return(dword10.wholeDword);
//}
//
//bool SubmissionQueue::setDword10(uint32_t dword){
//  dword10.wholeDword = dword;
//  return true;
//}
//
//uint32_t SubmissionQueue::getDword11(void){
//  return(dword11.wholeDword);
//}
//
//bool SubmissionQueue::setDword11(uint32_t dword){
//  dword11.wholeDword = dword;
//  return true;
//}
//
//Prp SubmissionQueue::getPrp1(void){
//  Prp prp;
//  prp.dptr0 = dptr.dptr0;
//  prp.dptr1 = dptr.dptr1;
//  return(prp);
//}
//
//bool SubmissionQueue::setPrp1(Prp prp){
//  dptr.dptr0 = prp.dptr0;
//  dptr.dptr1 = prp.dptr1;
//  return true;
//}
//
//bool SubmissionQueue::getSV(void){
//  return ((dword10.Word1.b1 >> 7) & 0x1);
//}
//
//bool SubmissionQueue::setSV(bool sv){
//  dword10.Word1.b1 = (dword10.Word1.b1 & 0x00) | ((sv << 7) & 0x80);
//  return true;
//}
//
//uint16_t SubmissionQueue::getFID(void){
//  return (dword10.Word0.wholeWord);
//}
//
//bool SubmissionQueue::setFID(uint16_t fid){
//  dword10.Word0.wholeWord = (dword10.Word0.wholeWord & 0x00) | fid;
//  return true;
//}
//
//uint16_t SubmissionQueue::getNoSQ(void){ 
//  return(dword11.Word0.wholeWord);
//}
//
//bool SubmissionQueue::setNoSQ(uint16_t noSq){
//  dword11.Word0.wholeWord = noSq;
//  return true;
//}
//
//uint16_t SubmissionQueue::getNoCQ(void){
//  return(dword11.Word1.wholeWord);
//}
//
//bool SubmissionQueue::setNoCQ(uint16_t noCq){
//  dword11.Word1.wholeWord = noCq;
//  return true;
//}
//
//uint64_t SubmissionQueue::getLBA(void){
//  return(dword10.wholeDword | ((dword11.wholeDword << 32 ) & 0xFFFFFFFF00000000ull));
//}
//
//bool SubmissionQueue::setLBA(uint64_t lba){
//  dword10.wholeDword = lba & 0x00000000FFFFFFFFull;
//  dword11.wholeDword = (lba >> 32) & 0x00000000FFFFFFFFull;
//  return true;
//}
//
//uint16_t SubmissionQueue::getNoOfLBA(void){
//  return(dword12.Word0.wholeWord);
//}
//
//bool SubmissionQueue::setNoOfLBA(uint16_t noOfLba){
//  dword12.Word0.wholeWord = noOfLba;
//  return true;
//}
//
//uint64_t SubmissionQueue::getBaseAdd(void){
//  uint64_t add;
//  Prp prp = getPrp1();
//  add = prp.dptr0.wholeDword | ((prp.dptr1.wholeDword << 32) & 0xFFFFFFFF00000000ull);
//  return(add);
//}
//
//bool SubmissionQueue::setBaseAdd(uint64_t add){
//  Prp prp;
//  prp.dptr0.wholeDword = add & 0x00000000FFFFFFFFull;
//  prp.dptr1.wholeDword = (add >> 32) & 0x00000000FFFFFFFFull;
//  setPrp1(prp);
//  return true;
//}
//
//uint32_t SubmissionQueue::getQSize(void){
//  return(dword10.Word1.wholeWord);
//}
//
//bool SubmissionQueue::setQSize(uint32_t qsize){
//  dword10.Word1.wholeWord = qsize;
//  return true;
//}
//
//uint16_t SubmissionQueue::getQID(void){
//  return(dword10.Word0.wholeWord);
//}
//
//bool SubmissionQueue::setQID(uint16_t qid){
//  dword10.Word0.wholeWord = qid;
//  return true;
//}
//
//uint16_t SubmissionQueue::getCQID(void){
//  return(dword11.Word1.wholeWord);
//}
//
//bool SubmissionQueue::setCQID(uint16_t cqid){
//  dword11.Word1.wholeWord = cqid;
//  return true;
//}

/*
template<class T>
CircularQueue<T>::CircularQueue(uint32_t baseAddress, uint32_t width){
  head = baseAddress;
  tail = baseAddress;
  queueStart = baseAddress;
  queueEnd = head + width + 1 ;
}

template<class T>
bool CircularQueue<T>::isFull(void){
  T* dummy;
  dummy = tail++;
  if (dummy == queueEnd) dummy = queueStart;
  if (head == dummy) return true;
  else return false;
}

template<class T>
bool CircularQueue<T>::isEmpty(void){
  if (head == tail) return true;
  else return false;
}

template<class T>
T* CircularQueue<T>::push(void){
  tail = tail++;
  if (tail == queueEnd) tail = queueStart;
  return tail;
	
}

template<class T>
T* CircularQueue<T>::getHead(void){
	return head;
}

template<class T>
T* CircularQueue<T>::getTail(void){
	return tail;
}

template<class T>
T* CircularQueue<T>::pop(void){
  head = head++;
  if (head == queueEnd) head = queueStart;
  return head;
}
*/


CircularQueue::CircularQueue(bool isSQ, uint32_t baseAddress, uint32_t size){
	this->mHead = baseAddress;
	this->mTail = baseAddress;
	this->mQueueStart = baseAddress;
	this->mIsSQ = isSQ;

	if (mIsSQ)
		//mQueueEnd = mHead + (size * SQ_WIDTH) + 1;
		mQueueEnd = mHead + (size * SQ_WIDTH);// +SQ_WIDTH;
	else
		//mQueueEnd = mHead + (size * CQ_WIDTH) + 1;
		mQueueEnd = mHead + (size * CQ_WIDTH);// +CQ_WIDTH;
}

bool CircularQueue::isFull(void){
	uint32_t queuePtr=mTail;
	if (mIsSQ)
		queuePtr += (SQ_WIDTH);
	else
		queuePtr += (CQ_WIDTH);

	if (queuePtr == mQueueEnd) queuePtr = mQueueStart;
	if (mHead == queuePtr) {
		//mTail = queuePtr;
		return true;
	}
	else return false;

	/*if ((mTail - mHead) == mQueueEnd){
		return true;
	}
	else{
		return false;
	}*/
}

bool CircularQueue::isEmpty(void){
	if (mHead == mTail) return true;
	else return false;
}


uint32_t CircularQueue::push(void){

	if (mIsSQ)
		mTail = mTail + (SQ_WIDTH);
	else
		mTail = mTail + (CQ_WIDTH);
	
	if (mTail == mQueueEnd) mTail = mQueueStart;
	return mTail;

	/*uint32_t tTail = mTail;
	tTail = mIsSQ ? (tTail + SQ_WIDTH):( tTail + CQ_WIDTH);
	if (tTail == mQueueEnd){
		tTail = mIsSQ ? (tTail - SQ_WIDTH) : (tTail - CQ_WIDTH);
	}
	mTail = tTail;
	return mTail;*/
}

uint32_t CircularQueue::getHead(void){
	return mHead;
}


uint32_t CircularQueue::getTail(void){
	return mTail;
}



uint32_t CircularQueue::pop(void){
	if (mIsSQ)
		mHead = mHead + (SQ_WIDTH);
	else
		mHead = mHead + (CQ_WIDTH);

	if (mIsSQ)
	{
		if (mHead == mQueueEnd) mHead = mQueueStart;
	}
	else {
		if (mHead == mQueueEnd) mHead = mQueueStart;
	}
	return mHead;
}

void CircularQueue::setHead(uint32_t head)
{
	mHead = head;
}

void CircularQueue::setTail(uint32_t tail)
{
	mTail = tail;
}