//
//  BackgroundChanger.h
//  NASA Desktop
//
//  Created by Ruffridge, Brandon J. (GRC-VA00) on 4/2/13.
//  Copyright (c) 2013 Ruffridge, Brandon J. (GRC-VA00). All rights reserved.
//

#import <Cocoa/Cocoa.h>

@interface BackgroundChanger : NSWorkspace {
    NSMutableData *receivedData;
}

@property (retain) NSMutableData *receivedData;

- (void) setWallpaper;

@end
