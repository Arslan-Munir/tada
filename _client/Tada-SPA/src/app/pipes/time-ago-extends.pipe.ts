import { Pipe } from '@angular/core';
import { TimeAgoPipe } from 'time-ago-pipe';

// tslint:disable-next-line: use-pipe-transform-interface
@Pipe({
  name: 'timeAgo',
  pure: false
})
export class TimeAgoExtendsPipe extends TimeAgoPipe {

}
