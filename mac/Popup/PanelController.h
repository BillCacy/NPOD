#import "BackgroundView.h"
#import "StatusItemView.h"

@class PanelController;

@protocol PanelControllerDelegate <NSObject>

@optional

- (StatusItemView *)statusItemViewForPanelController:(PanelController *)controller;

@end

#pragma mark -

@interface PanelController : NSWindowController <NSWindowDelegate>
{
    BOOL _hasActivePanel;
    __unsafe_unretained BackgroundView *_backgroundView;
    __unsafe_unretained id<PanelControllerDelegate> _delegate;
    __unsafe_unretained NSSearchField *_searchField;
    __unsafe_unretained NSTextField *_iotdTitle;
    __unsafe_unretained NSTextView *_iotdDescription;
    __unsafe_unretained NSTextField *_iotdUpdateStatus;
}

@property (nonatomic, unsafe_unretained) IBOutlet BackgroundView *backgroundView;
@property (nonatomic, unsafe_unretained) IBOutlet NSSearchField *searchField;
@property (nonatomic, unsafe_unretained) IBOutlet NSTextField *iotdTitle;
@property (nonatomic, unsafe_unretained) IBOutlet NSTextView *iotdDescription;
@property (nonatomic, unsafe_unretained) IBOutlet NSTextField *iotdUpdateStatus;
@property (nonatomic, strong) NSString *iotdTitleText;
@property (nonatomic, strong) NSString *iotdDescriptionText;
//@property (nonatomic, unsafe_unretained) NSArray *titleDesc;

@property (nonatomic) BOOL hasActivePanel;
@property (nonatomic, unsafe_unretained, readonly) id<PanelControllerDelegate> delegate;

- (id)initWithDelegate:(id<PanelControllerDelegate>)delegate;

- (void)openPanel;
- (void)closePanel;

@end
