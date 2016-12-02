/*******************************************************************
 * File : Config.h
 *
 * Copyright(C) HCL Technologies Ltd. 
 * 
 * ALL RIGHTS RESERVED.
 *
 * Description: This file contains all the common header for all configuration 
 * Related Specifications: 
 *
 ********************************************************************/
#pragma once

#define		BA_CNTL			0x10000000
#define		BA_MEM			0x0
#define		BA_ASQ			0x00010000
#define		BA_ACQ			0x00020000
#define		BA_SQ0			0x00030000
#define		BA_CQ0			0x00040000
#define		BA_DATA			0x00050000

#define		SZ_ASQ			2
#define		SZ_ACQ			2
#define		SZ_SQ0			4
#define		SZ_CQ0			4
#define		WT_CQ			500

#define		PENDING_QUEUE_DEPTH  20



