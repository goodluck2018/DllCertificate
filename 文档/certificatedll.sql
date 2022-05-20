/*
Navicat MySQL Data Transfer

Source Server         : localhost
Source Server Version : 50717
Source Host           : localhost:3306
Source Database       : certificatedll

Target Server Type    : MYSQL
Target Server Version : 50717
File Encoding         : 65001

Date: 2022-05-20 16:24:55
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `tcertificaterecord`
-- ----------------------------
DROP TABLE IF EXISTS `tcertificaterecord`;
CREATE TABLE `tcertificaterecord` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `DeviceCode` varchar(200) NOT NULL COMMENT '设备编号',
  `SerialNumber` varchar(200) NOT NULL COMMENT '序列号',
  `Validate` varchar(25) DEFAULT NULL COMMENT '有效期 (yyyy-MM-dd HH:mm:ss)',
  `CreateTime` varchar(25) DEFAULT NULL COMMENT '认证时间 (yyyy-MM-dd HH:mm:ss)',
  `Operator` varchar(100) DEFAULT NULL COMMENT '操作员',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=25 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of tcertificaterecord
-- ----------------------------
INSERT INTO `tcertificaterecord` VALUES ('1', '', '', '2122-05-20 12:32:24', null, '');
INSERT INTO `tcertificaterecord` VALUES ('2', 'aaaa-aaaa', 'ZF71R-DMX85-08DQY-8YMNC-PPHV8', '2122-05-20 12:36:59', null, '');
INSERT INTO `tcertificaterecord` VALUES ('3', 'aaaa-aaaa', 'ZF71R-DMX85-08DQY-8YMNC-PPHV8', '2122-05-20 12:43:04', null, '');
INSERT INTO `tcertificaterecord` VALUES ('4', 'aaaa-aaaa', 'ZF71R-DMX85-08DQY-8YMNC-PPHV8', '2122-05-20 12:43:35', null, '');
INSERT INTO `tcertificaterecord` VALUES ('5', 'aaaa-aaaa', 'ZF71R-DMX85-08DQY-8YMNC-PPHV8', '2122-05-20 12:55:46', null, '');
INSERT INTO `tcertificaterecord` VALUES ('6', 'aaaa-aaaa', 'ZF71R-DMX85-08DQY-8YMNC-PPHV8', '2122-05-20 15:29:08', null, '');
INSERT INTO `tcertificaterecord` VALUES ('7', 'aaaa-aaaa', 'ZF71R-DMX85-08DQY-8YMNC-PPHV8', '2122-05-20 15:29:16', null, '');
INSERT INTO `tcertificaterecord` VALUES ('8', 'aaaa-aaaa', 'ZF71R-DMX85-08DQY-8YMNC-PPHV8', '2122-05-20 15:30:34', null, '');
INSERT INTO `tcertificaterecord` VALUES ('9', 'aaaa-aaaa', 'ZF71R-DMX85-08DQY-8YMNC-PPHV8', '2122-05-20 15:31:18', null, '');
INSERT INTO `tcertificaterecord` VALUES ('10', 'BFEBFBFF000806C1', 'ZF71R-DMX85-08DQY-8YMNC-PPHV8', '2122-05-20 16:07:18', null, '');
INSERT INTO `tcertificaterecord` VALUES ('11', 'BFEBFBFF000806C1', 'ZF71R-DMX85-08DQY-8YMNC-PPHV8', '2122-05-20 16:09:52', null, '');
INSERT INTO `tcertificaterecord` VALUES ('12', 'BFEBFBFF000806C1', 'ZF71R-DMX85-08DQY-8YMNC-PPHV8', '2122-05-20 16:10:24', null, '');
INSERT INTO `tcertificaterecord` VALUES ('13', 'BFEBFBFF000806C1', 'aaaa', '2122-05-20 16:11:23', null, '');
INSERT INTO `tcertificaterecord` VALUES ('14', 'BFEBFBFF000806C1', '1231231', '2122-05-20 16:11:48', null, '');
INSERT INTO `tcertificaterecord` VALUES ('15', 'BFEBFBFF000806C1', '1231312', '2122-05-20 16:12:39', null, '');
INSERT INTO `tcertificaterecord` VALUES ('16', 'BFEBFBFF000806C1', '112112', '2122-05-20 16:14:10', null, '');
INSERT INTO `tcertificaterecord` VALUES ('17', 'BFEBFBFF000806C1', 'ZF71R-DMX85-08DQY-8YMNC-PPHV8', '2122-05-20 16:16:54', null, '');
INSERT INTO `tcertificaterecord` VALUES ('18', 'BFEBFBFF000806C1', 'ZF71R-DMX85-08DQY-8YMNC-1', '2122-05-20 16:17:06', null, '');
INSERT INTO `tcertificaterecord` VALUES ('19', 'BFEBFBFF000806C1', 'ZF71R-DMX85-08DQY-8YMNC-PPHV8', '2122-05-20 16:17:10', null, '');
INSERT INTO `tcertificaterecord` VALUES ('20', 'BFEBFBFF000806C1', 'ZF71R-DMX85-08DQY-8YMN', '2122-05-20 16:17:33', null, '');
INSERT INTO `tcertificaterecord` VALUES ('21', 'BFEBFBFF000806C1', 'ZF71R-DMX85-08DQY-8YMN', '2122-05-20 16:17:37', null, '');
INSERT INTO `tcertificaterecord` VALUES ('22', 'BFEBFBFF000806C1', 'ZF71R-DMX85-08DQY-8YMNC-PPHV8', '2122-05-20 16:17:42', null, '');
INSERT INTO `tcertificaterecord` VALUES ('23', 'BFEBFBFF000806C1', 'ZF71R-DMX85-08DQY-8YMNC-PPHV8', '2122-05-20 16:20:27', null, '');
INSERT INTO `tcertificaterecord` VALUES ('24', 'BFEBFBFF000806C1', 'ZF71R-DMX85-08DQY-8YMNC-PPHV8', '2122-05-20 16:23:39', '2022-05-20 16:23:39', '');

-- ----------------------------
-- Table structure for `tserialnumber`
-- ----------------------------
DROP TABLE IF EXISTS `tserialnumber`;
CREATE TABLE `tserialnumber` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `SerialNumber` varchar(255) NOT NULL COMMENT '序列号',
  `Validate` varchar(25) DEFAULT NULL COMMENT '有效期',
  `CreateTime` varchar(25) DEFAULT NULL COMMENT '生成时间',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of tserialnumber
-- ----------------------------
INSERT INTO `tserialnumber` VALUES ('1', 'ZF71R-DMX85-08DQY-8YMNC-PPHV8', '2122-01-01 00:00:00', '2022-01-01 00:00:0');
INSERT INTO `tserialnumber` VALUES ('2', 'YF390-0HF8P-M81RQ-2DXQE-M2UT6', '2122-01-01 00:00:00', '2022-01-01 00:00:0');
