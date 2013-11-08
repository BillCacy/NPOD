#import "ApplicationDelegate.h"
#import "BackgroundChanger.h"

@implementation ApplicationDelegate

@synthesize panelController = _panelController;
@synthesize menubarController = _menubarController;
@synthesize iotdTitle = _iotdTitle;
@synthesize iotdDescription = _iotdDescription;

#pragma mark -

- (void)dealloc
{
    [_panelController removeObserver:self forKeyPath:@"hasActivePanel"];
}

#pragma mark -

void *kContextActivePanel = &kContextActivePanel;

- (void)observeValueForKeyPath:(NSString *)keyPath ofObject:(id)object change:(NSDictionary *)change context:(void *)context
{
    if (context == kContextActivePanel) {
        self.menubarController.hasActiveIcon = self.panelController.hasActivePanel;
    }
    else {
        [super observeValueForKeyPath:keyPath ofObject:object change:change context:context];
    }
}

#pragma mark - NSApplicationDelegate

- (void)applicationDidFinishLaunching:(NSNotification *)notification
{
    // add app to login items.
    [self addAppAsLoginItem];
    
    // Install icon into the menu bar
    self.menubarController = [[MenubarController alloc] init];
    
    [self updateWallpaper];
    
    //calculate the n seconds until 10:30am EST
    
    //get current date and time.
    // either as a timeinterval since a reference date, or as a date object.
    
    //get the hours and minutes of the current time and determine if the time is before or after 10:30 am.
    //get a string of the month day and year of the current date.
    // add to the string the gmt hours
    // NSDate *exp=[[NSDate alloc] initWithString:@"2011-01-07 10:30:00 -0500"]
    // get the timeinterval between the current datetime and the 1030 datetime.
    // if it is greater than zero set the timer to happen after that interval.
    // otherwise add 1 day to the 1030 datetime calculate the interval again and set the timer to happen after that interval.
    
    //start a timer to check for a new wallpaper after n seconds. timer should repeat every 24 hours.
    NSDate *now = [NSDate date];
    
    NSDateFormatter *dateFormatter = [[NSDateFormatter alloc] init];
    [dateFormatter setDateFormat:@"yyyy-MM-dd"];
    NSLocale *usLocale = [[NSLocale alloc] initWithLocaleIdentifier:@"en_US"];
    [dateFormatter setLocale:usLocale];
    
    NSString *formattedDateString = [dateFormatter stringFromDate:now];
    NSString *string1030 = [formattedDateString stringByAppendingString:@" 10:30:00 -0500"];
    NSDate *now1030 = [NSDate dateWithString:string1030];
    //NSLog(@"formattedDateString: %@", formattedDateString);
    NSTimeInterval timeTil1030 = [now1030 timeIntervalSinceDate:now];
    
    if(timeTil1030 <= 0) {
        now1030 = [now1030 dateByAddingTimeInterval:86400];
        timeTil1030 = [now1030 timeIntervalSinceDate:now]; 
    }
    [NSTimer scheduledTimerWithTimeInterval:timeTil1030 target:self selector:@selector(update1030:) userInfo:@{ @"StartDate" : [NSDate date] } repeats:NO];
    
    //NSLog(@"timeTil1030:%f", timeTil1030);
    
}

- (void)update1030:(NSTimer*)theTimer {
    NSDate *startDate = [[theTimer userInfo] objectForKey:@"StartDate"];
    NSLog(@"Timer started on %@", startDate);
    [self updateWallpaper];
    [NSTimer scheduledTimerWithTimeInterval:86400 target:self selector:@selector(update24:) userInfo:@{ @"StartDate" : [NSDate date] } repeats:YES];
}

- (void)update24:(NSTimer*)theTimer {
    NSDate *startDate = [[theTimer userInfo] objectForKey:@"StartDate"];
    NSLog(@"Timer started on %@", startDate);
    [self updateWallpaper];
}

- (void)updateWallpaper {
    //Update Wallpaper.
    BackgroundChanger *bc = [BackgroundChanger new];
    NSArray *titleDesc = [bc setWallpaper:nil];
    if(titleDesc) {
        _iotdTitle = [titleDesc objectAtIndex:0];
        _iotdDescription = [titleDesc objectAtIndex:1];
    }
    else {
        _iotdTitle = @"There was a problem downloading the image.";
        _iotdDescription = @"";
    }
}

- (NSApplicationTerminateReply)applicationShouldTerminate:(NSApplication *)sender
{
    // Explicitly remove the icon from the menu bar
    self.menubarController = nil;
    return NSTerminateNow;
}

#pragma mark - Actions

- (IBAction)togglePanel:(id)sender
{
    self.menubarController.hasActiveIcon = !self.menubarController.hasActiveIcon;
    self.panelController.hasActivePanel = self.menubarController.hasActiveIcon;
}

#pragma mark - Public accessors

- (PanelController *)panelController
{
    if (_panelController == nil) {
        _panelController = [[PanelController alloc] initWithDelegate:self];
        [_panelController addObserver:self forKeyPath:@"hasActivePanel" options:0 context:kContextActivePanel];
        _panelController.iotdTitleText = _iotdTitle;
        _panelController.iotdDescriptionText = _iotdDescription;
    }
    return _panelController;
}

#pragma mark - PanelControllerDelegate

- (StatusItemView *)statusItemViewForPanelController:(PanelController *)controller
{
    return self.menubarController.statusItemView;
}

-(void) addAppAsLoginItem{
	NSString * appPath = [[NSBundle mainBundle] bundlePath];
    
	// This will retrieve the path for the application
	// For example, /Applications/test.app
	CFURLRef url = (__bridge CFURLRef)[NSURL fileURLWithPath:appPath];
    
	// Create a reference to the shared file list.
    // We are adding it to the current user only.
    // If we want to add it all users, use
    // kLSSharedFileListGlobalLoginItems instead of
    //kLSSharedFileListSessionLoginItems
	LSSharedFileListRef loginItems = LSSharedFileListCreate(NULL,
                                                            kLSSharedFileListSessionLoginItems, NULL);
	if (loginItems) {
		//Insert an item to the list.
		LSSharedFileListItemRef item = LSSharedFileListInsertItemURL(loginItems,
                                                                     kLSSharedFileListItemLast, NULL, NULL,
                                                                     url, NULL, NULL);
		if (item){
			CFRelease(item);
        }
	}
    
	CFRelease(loginItems);
}

@end
