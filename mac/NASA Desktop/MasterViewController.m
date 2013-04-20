//
//  MasterViewController.m
//  NASA Desktop
//
//  Created by Ruffridge, Brandon J. (GRC-VA00) on 4/2/13.
//  Copyright (c) 2013 Ruffridge, Brandon J. (GRC-VA00). All rights reserved.
//

#import "MasterViewController.h"
#import "BackgroundChanger.h"

@interface MasterViewController ()

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
    [bc setWallpaper];
}

@end
