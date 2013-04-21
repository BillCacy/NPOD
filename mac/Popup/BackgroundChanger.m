#import "BackgroundChanger.h"

@implementation BackgroundChanger

@synthesize receivedData;

-(NSArray *)setWallpaper
{
    
    // get first <ig> under root. This is the latest image of the day.
    // get the first <ap>. This contains a link to the xml file for the image.
    // append http://www.nasa.gov to the value.
    // download the file. root is channel
    // get the <href> in <image> - <size> with <type>Full_Size</type>
    // this is the file to download to the client and set as wallpaper.
    
    //download http://www.nasa.gov/multimedia/imagegallery/iotdxml.xml
    NSError *err = nil;
    NSArray *titleDesc = nil;
    NSURL *myURL = [NSURL URLWithString:@"http://www.nasa.gov/multimedia/imagegallery/iotdxml.xml"];
    NSXMLDocument *iotdxml = [[NSXMLDocument alloc] initWithContentsOfURL:myURL options:0 error:&err];
    // Root is rss/channel
    // get first <ig> under root. This is the latest image of the day.
    // get the first <ap>. This contains a link to the xml file for the image.
    NSArray *nodes = [iotdxml nodesForXPath:@"./rss[1]/channel[1]/ig[1]/ap[1]"
                                      error:&err];
    if ([nodes count] > 0 ) {
        // do something with element
        NSXMLElement *latestImgXML = [nodes objectAtIndex:0];
        NSString *latestImgXMLHref = [latestImgXML stringValue];
        
        // append http://www.nasa.gov to the value.
        NSString *baseURL = @"http://www.nasa.gov";
        latestImgXMLHref = [baseURL stringByAppendingString:latestImgXMLHref];
        latestImgXMLHref = [latestImgXMLHref stringByAppendingString:@".xml"];
        NSLog(@"%@",latestImgXMLHref);
        
        
        // download the file.
        myURL = [NSURL URLWithString:latestImgXMLHref];
        iotdxml = [[NSXMLDocument alloc] initWithContentsOfURL:myURL options:0 error:&err];
        
        NSString *iotdTitleString = @"";
        NSString *iotdDescriptionString = @"";
        
        // get the title
        nodes = [iotdxml nodesForXPath:@"./rss[1]/channel[1]/title[1]"
                                 error:&err];
        if ([nodes count] > 0 ) {
            // do something with element
            NSXMLElement *iotdTitleElement = [nodes objectAtIndex:0];
            iotdTitleString = [iotdTitleElement stringValue];
        }
        
        // get the description
        nodes = [iotdxml nodesForXPath:@"./rss[1]/channel[1]/description[1]"
                                 error:&err];
        if ([nodes count] > 0 ) {
            // do something with element
            NSXMLElement *iotdDescriptionElement = [nodes objectAtIndex:0];
            iotdDescriptionString = [iotdDescriptionElement stringValue];
        }
        
        titleDesc = [NSArray arrayWithObjects:iotdTitleString, iotdDescriptionString, nil ];
        
        //[iotdTitle setStringValue:iotdTitleString];
        //[iotdDescription setStringValue:iotdDescriptionString];
        
        // get the <href> in <image> - <size> with <type>Full_Size</type>
        nodes = [iotdxml nodesForXPath:@"./rss[1]/channel[1]/image[1]/size[type=\"Full_Size\"][1]/href"
                                 error:&err];
        if ([nodes count] > 0 ) {
            // do something with element
            NSXMLElement *iotdHrefElement = [nodes objectAtIndex:0];
            NSString *iotdHrefString = [iotdHrefElement stringValue];
            
            // append http://www.nasa.gov to the value.
            iotdHrefString = [baseURL stringByAppendingString:iotdHrefString];
            NSLog(@"%@",iotdHrefString);
            
            // Download the file and set as wallpaper.
            // Create the request.
            NSURLRequest *theRequest=[NSURLRequest requestWithURL:[NSURL URLWithString:iotdHrefString]
                                                      cachePolicy:NSURLRequestUseProtocolCachePolicy
                                                  timeoutInterval:60.0];
            // create the connection with the request
            // and start loading the data
            NSURLConnection *theConnection=[[NSURLConnection alloc] initWithRequest:theRequest delegate:self];
            if (theConnection) {
                // Create the NSMutableData to hold the received data.
                // receivedData is an instance variable declared elsewhere.
                receivedData = [NSMutableData data];
            } else {
                // Inform the user that the connection failed.
            }
        }
        if (err != nil) {
            NSLog(@"%@",[err localizedDescription]);
        }
    }
    if (err != nil) {
        NSLog(@"%@",[err localizedDescription]);
    }
    return titleDesc;
}

- (void)connection:(NSURLConnection *)connection didReceiveResponse:(NSURLResponse *)response
{
    // This method is called when the server has determined that it
    // has enough information to create the NSURLResponse.
    
    // It can be called multiple times, for example in the case of a
    // redirect, so each time we reset the data.
    
    // receivedData is an instance variable declared elsewhere.
    [receivedData setLength:0];
}

- (void)connection:(NSURLConnection *)connection didReceiveData:(NSData *)data
{
    // Append the new data to receivedData.
    // receivedData is an instance variable declared elsewhere.
    [receivedData appendData:data];
}

- (void)connection:(NSURLConnection *)connection
  didFailWithError:(NSError *)error
{
    // release the connection, and the data object
    //[connection release];
    // receivedData is declared as a method instance elsewhere
    //[receivedData release];
    
    // inform the user
    NSLog(@"Connection failed! Error - %@ %@",
          [error localizedDescription],
          [[error userInfo] objectForKey:NSURLErrorFailingURLStringErrorKey]);
}

- (void)connectionDidFinishLoading:(NSURLConnection *)connection
{
    // do something with the data
    // receivedData is declared as a method instance elsewhere
    NSLog(@"Succeeded! Received %ld bytes of data",[receivedData length]);
    NSError *err = nil;
    NSWorkspace *sws = [NSWorkspace sharedWorkspace];
    
    //check file if 1 write to 2 and delete 1. if 2 write to 1 and delete 2.
    
    NSString *writeToFile = [@"~/Pictures/nasaiotd.jpg" stringByExpandingTildeInPath];
    NSURL *image = [NSURL fileURLWithPath:writeToFile];
    if([NSData dataWithContentsOfURL:image]) {
        [[NSFileManager defaultManager] removeItemAtURL:image error:&err];
        writeToFile = [@"~/Pictures/nasaiotd-alt.jpg" stringByExpandingTildeInPath];
    }
    else {
        NSString *writeToFile2 = [@"~/Pictures/nasaiotd-alt.jpg" stringByExpandingTildeInPath];
        image = [NSURL fileURLWithPath:writeToFile2];
        if([NSData dataWithContentsOfURL:image]) {
            [[NSFileManager defaultManager] removeItemAtURL:image error:&err];
            writeToFile = [@"~/Pictures/nasaiotd.jpg" stringByExpandingTildeInPath];
        }
    }
    
    if ([receivedData writeToFile:writeToFile
                       atomically:YES])
    {
        // It was successful, do stuff here
        NSURL *image = [NSURL fileURLWithPath:writeToFile];
        [NSData dataWithContentsOfURL:image];
        for (NSScreen *screen in [NSScreen screens]) {
            NSDictionary *opt = [sws desktopImageOptionsForScreen:screen];
            [sws setDesktopImageURL:image forScreen:screen options:opt error:&err];
            if (err) {
                NSLog(@"%@",[err localizedDescription]);
            }else{
                NSNumber *scr = [[screen deviceDescription] objectForKey:@"NSScreenNumber"];
                NSLog(@"Set %@ for space %i on screen %@",[image path],1,scr);
            }
        }
    }
    else
    {
        // There was a problem writing the file
    }
}

@end

