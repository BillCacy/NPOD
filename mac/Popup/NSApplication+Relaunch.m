//
//  NSApplication+Relaunch.m
//  NPOD
//
//  Created by Ruffridge, Brandon J. (GRC-VA00) on 11/15/13.
//
//

#import "NSApplication+Relaunch.h"

@implementation NSApplication (Relaunch)

- (void)relaunch:(id)sender
{
	NSString *daemonPath = [[NSBundle mainBundle] pathForResource:NSApplicationRelaunchDaemon ofType:nil];
	[NSTask launchedTaskWithLaunchPath:daemonPath arguments:[NSArray arrayWithObject:[[NSBundle mainBundle] bundlePath]]];
	[self terminate:sender];
}

@end
