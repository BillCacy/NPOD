#import "MenubarController.h"
#import "PanelController.h"

@interface ApplicationDelegate : NSObject <NSApplicationDelegate, PanelControllerDelegate>

@property (retain) NSMutableData *receivedData;


@property (nonatomic, strong) MenubarController *menubarController;
@property (nonatomic, strong, readonly) PanelController *panelController;
@property (nonatomic, strong) NSString *iotdTitle;
@property (nonatomic, strong) NSString *iotdDescription;

- (IBAction)togglePanel:(id)sender;

@end
