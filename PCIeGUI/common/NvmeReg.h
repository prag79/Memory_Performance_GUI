/*******************************************************************
 * File : NvmeReg.h
 *
 * Copyright(C) HCL Technologies Ltd. 
 * 
 * ALL RIGHTS RESERVED.
 *
 * Description: This file contains all the common header for NVME Queue 
 * Related Specifications: 
 *
 ********************************************************************/
#pragma once
#include <cstdint>
#include "config.h"


#define   CAP_ADD     (BA_CNTL + 0x0000)
#define    VS_ADD     (BA_CNTL + 0x0008)
#define INTMS_ADD     (BA_CNTL + 0x000C)
#define INTMC_ADD     (BA_CNTL + 0x0010)
#define    CC_ADD     (BA_CNTL + 0x0014)
#define   RES_ADD     (BA_CNTL + 0x0018)
#define  CSTS_ADD     (BA_CNTL + 0x001C)
#define  NSSR_ADD     (BA_CNTL + 0x0020)
#define   AQA_ADD     (BA_CNTL + 0x0024)
#define   ASQ_ADD     (BA_CNTL + 0x0028)
#define   ACQ_ADD     (BA_CNTL + 0x0030)
#define  SQ0TDBL_ADD  (BA_CNTL + 0x1000 +  4)
#define  CQ0HDBL_ADD  (BA_CNTL + 0x1000 +  8)
#define  SQ1TDBL_ADD  (BA_CNTL + 0x1000 + 12)
#define  CQ1HDBL_ADD  (BA_CNTL + 0x1000 + 16)

//CAP Register(64 bit)
#define SET_CAP_MPSMAX(reg, value) ((reg & 0xFF0FFFFFFFFFFFFFull)| ((value << 52) & 0x00F0000000000000ull))  
#define GET_CAP_MPSMAX(reg)        ((reg >> 52) & 0xF)
#define SET_CAP_MPSMIN(reg, value) ((reg & 0xFFF0FFFFFFFFFFFFull)| ((value << 48) & 0x000F000000000000ull))  
#define GET_CAP_MPSMIN(reg)        ((reg >> 48) & 0xF)
#define    SET_CAP_CSS(reg, value) ((reg & 0xFFFFE01FFFFFFFFFull)| ((value << 37) & 0x00001FE000000000ull))  
#define    GET_CAP_CSS(reg)        ((reg >> 37) & 0xFF)
#define     SET_CAP_TO(reg, value) ((reg & 0xFFFFFFFF00FFFFFFull)| ((value << 24) & 0x00000000FF000000ull))  
#define     GET_CAP_TO(reg)        ((reg >> 24) & 0xFF)
#define     SET_CAP_PC(reg, value) ((reg & 0xFFFFFFFFFFFFFEFFull)| ((value << 24) & 0x0000000000000100ull))  
#define     GET_CAP_PC(reg)        ((reg >> 16) & 0x1)
#define   SET_CAP_MQES(reg, value) ((reg & 0xFFFFFFFFFFFFF000ull)| (value & 0x0000000000000FFFull))  
#define   GET_CAP_MQES(reg)        (reg & 0xFFF)

//CC Register (32 bit)
#define     SET_CC_IOCQES(reg, value) ((reg & 0xFF0FFFFF) | ((value << 20) & 0x00F00000) )  
#define     GET_CC_IOCQES(reg)        ((reg >> 20) & 0xF)
#define     SET_CC_IOSQES(reg, value) ((reg & 0xFFF0FFFF) | ((value << 16) & 0x000F0000) )  
#define     GET_CC_IOSQES(reg)        ((reg >> 16) & 0xF)
#define        SET_CC_MPS(reg, value) ((reg & 0xFFFFF87F) | ((value << 7) & 0x00000780) )  
#define        GET_CC_MPS(reg)        ((reg >> 7) & 0xF)
#define        SET_CC_CSS(reg, value) ((reg & 0xFFFFFF8F) | ((value << 4) & 0x00000070) )  
#define        GET_CC_CSS(reg)        ((reg >> 4) & 0x7)
#define         SET_CC_EN(reg, value) ((reg & 0xFFFFFFFE) | value )  
#define         GET_CC_EN(reg)        (reg & 0x1)

//CSTS (32 bit)
#define    SET_CSTS_RDY(reg, value) ((reg & 0xFFFFFFFE) | value )  
#define    GET_CSTS_RDY(reg)        (reg & 0x1)

//AQA (32 bit)
#define   SET_AQA_ACQS(reg, value) ((reg & 0xF000FFFF)| ((value << 16) & 0x0FFF0000))  
#define   GET_AQA_ACQS(reg)        ((reg >> 16) & 0xFFF)
#define   SET_AQA_ASQS(reg, value) ((reg & 0xFFFFF000)| (value & 0x0FFF))  
#define   GET_AQA_ASQS(reg)        (reg & 0xFFF)

//ASQ (64 bit)
#define   SET_ASQB(reg, value) ((reg & 0x0000000000000000ull)| ((value << 12) & 0xFFFFFFFFFFFFF000ull))  
#define   GET_ASQB(reg)        ((reg >> 12) & 0xFFFFFFFFFFFFFFFFull)

//ACQ (64 bit)
#define   SET_ACQB(reg, value) ((reg & 0x0000000000000000ull)| ((value << 12) & 0xFFFFFFFFFFFFF000ull))  
#define   GET_ACQB(reg)        ((reg >> 12) & 0xFFFFFFFFFFFFFFFFull)
