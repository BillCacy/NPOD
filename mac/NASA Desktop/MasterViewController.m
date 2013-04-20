//
//  MasterViewController.m
//  NASA Desktop
//

#import "MasterViewController.h"
#import "BackgroundChanger.h"

@interface MasterViewController ()

@property (weak) IBOutlet NSTextField *iotdTitle;
@property (weak) IBOutlet NSTextField *iotdDescription;

@end

@implementation MasterViewController

- (id)initWithNibName:(NSString *)nibNameOrNil bundle:(NSBundle *)nibBundleOrNil
{
    self = [super initWithNibName:nibNameOrNil bundle:nibBundleOrNil];
    if (self) {
        // Initialization code here.
    }
    
    return self;
}

-(IBAction)changeBackground:(id)sender
{
    BackgroundChanger *bc = [BackgroundChanger new];
    [bc setWallpaper:_iotdTitle getIotdDescription:_iotdDescription];
}

@end
