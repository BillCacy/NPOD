//
//  NSApplication+Relaunch.h
//  NPOD
//
//  Created by Ruffridge, Brandon J. (GRC-VA00) on 11/15/13.
//
//

#import <Cocoa/Cocoa.h>

#define NSApplicationRelaunchDaemon @"relaunch"

@interface NSApplication (Relaunch)

- (void)relaunch:(id)sender;

@end