//
//  BackgroundChanger.h
//  NASA Desktop
//

#import <Cocoa/Cocoa.h>

@interface BackgroundChanger : NSWorkspace {
    NSMutableData *receivedData;
}

@property (retain) NSMutableData *receivedData;

- (void)setWallpaper:(NSTextField *)iotdTitle getIotdDescription:(NSTextField *)iotdDescription;

@end
