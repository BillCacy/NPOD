#import <Cocoa/Cocoa.h>

@interface BackgroundChanger : NSWorkspace {
    NSMutableData *receivedData;
}

@property (retain) NSMutableData *receivedData;

- (NSArray *)setWallpaper;

@end