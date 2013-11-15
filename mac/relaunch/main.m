//
//  main.m
//  relaunch
//

//
//

#import <Cocoa/Cocoa.h>

int main(int argc, const char * argv[])
{
    BOOL success = 0;
    @autoreleasepool {
        
        // wait a sec, to be safe
        sleep(1);
        
        NSString *appPath = [NSString stringWithCString:argv[1] encoding:NSUTF8StringEncoding];
        success = [[NSWorkspace sharedWorkspace] openFile:[appPath stringByExpandingTildeInPath]];
        
        if (!success)
            NSLog(@"Error: could not relaunch application at %@", appPath);
        
    }
    return (success) ? 0 : 1;
}

