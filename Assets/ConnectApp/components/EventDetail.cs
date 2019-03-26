using System.Collections.Generic;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace ConnectApp.components {
    public class EventDetail : StatelessWidget {
        public EventDetail(
            Key key = null,
            IEvent eventObj = null
        ) : base(key) {
            this.eventObj = eventObj;
        }

        private readonly IEvent eventObj;

        public override Widget build(BuildContext context) {
            return new Container(child: _buildContent(context));
        }

        private Widget _buildContent(BuildContext context) {
            var items = new List<Widget>();
            items.Add(_buildContentHead());
            items.AddRange(ArticleDescription.map(context,eventObj.content, contentMap: eventObj.contentMap));
            return new Flexible(
                child: ListView.builder(
                    physics: new AlwaysScrollableScrollPhysics(),
                    itemCount: items.Count,
                    itemBuilder: (cxt, index) => items[index]
                )
            );
        }

        private Widget _buildContentHead() {
            var user = eventObj.user ?? new User();
            return new Container(
                padding: EdgeInsets.symmetric(horizontal: 16),
                margin: EdgeInsets.only(top: 16,bottom:44),
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Text(
                            eventObj.title ?? "",
                            style: CTextStyle.H3
                        ),
                        new Container(
                            margin: EdgeInsets.only(top: 20),
                            child: new Row(
                                children: new List<Widget> {
                                    new Container(
                                        margin: EdgeInsets.only(right: 8),
                                        child: new Avatar(
                                            user.id ?? "",
                                            32
                                        )
                                    ),
                                    new Column(
                                        mainAxisAlignment: MainAxisAlignment.center,
                                        crossAxisAlignment: CrossAxisAlignment.start,
                                        children: new List<Widget> {
                                            new Text(
                                                user.fullName ?? "",
                                                style: CTextStyle.PMediumBody
                                            ),
                                            new Text(
                                                $"{DateConvert.DateStringFromNow(eventObj.createdTime)}发布",
                                                style: CTextStyle.PSmallBody3
                                            )
                                        }
                                    )
                                }
                            )
                        )
                    }
                )
            );
        }

        private Widget _buildContentDetail() {
            return new Container(
                margin: EdgeInsets.only(top: 44),
                child: new Column(
                    mainAxisAlignment: MainAxisAlignment.start,
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new EventDescription(content: eventObj.content, contentMap: eventObj.contentMap)
                    }
                )
            );
        }

        private Widget _buildContentLecturerList() {
            var hosts = eventObj.hosts;
            if (hosts == null || hosts.Count == 0) return new Container();
            var hostItems = new List<Widget>();
            hosts.ForEach(host => { hostItems.Add(_buildLecture(host)); });
            return new Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: new List<Widget> {
                    new Padding(
                        padding: EdgeInsets.fromLTRB(16, 0, 0, 16),
                        child: new Text(
                            "讲师",
                            style: new TextStyle(
                                color: Color.black,
                                fontFamily: "Roboto-Medium",
                                fontSize: 20
                            )
                        )
                    ),
                    new Container(
                        height: 238,
                        margin: EdgeInsets.only(bottom: 64),
                        padding: EdgeInsets.fromLTRB(16, 0, 16, 16),
                        child: new ListView(
                            physics: new AlwaysScrollableScrollPhysics(),
                            scrollDirection: Axis.horizontal,
                            children: hostItems
                        )
                    )
                }
            );
        }

        private static Widget _buildLecture(User host) {
            return new Container(
                width: 212,
                padding: EdgeInsets.only(top: 24),
                margin: EdgeInsets.only(right: 16),
                decoration: new BoxDecoration(
                    Color.fromARGB(255, 76, 76, 76)
                ),
                child: new Column(
                    children: new List<Widget> {
                        new Container(
                            child: new Avatar(
                                host.id,
                                80
                            )
                        ),
                        new Container(
                            margin: EdgeInsets.only(top: 12),
                            padding: EdgeInsets.symmetric(horizontal: 16),
                            child: new Text(
                                host.fullName, 
                                style: new TextStyle(color: Color.white, fontSize: 16)
                            )
                        ),
                        new Container(
                            margin: EdgeInsets.only(top: 4),
                            padding: EdgeInsets.symmetric(horizontal: 16),
                            child: new Text(
                                host.title,
                                maxLines: 1, 
                                overflow: TextOverflow.ellipsis,
                                style: new TextStyle(color: new Color(0xFF959595), fontSize: 16)
                            )
                        ),
                        new Container(
                            margin: EdgeInsets.only(top: 12),
                            padding: EdgeInsets.symmetric(horizontal: 14),
                            child: new Text(
                                host.description,
                                textAlign: TextAlign.center,
                                maxLines: 2,
                                overflow: TextOverflow.ellipsis,
                                style: new TextStyle(color: new Color(0xFFD8D8D8), fontSize: 16)
                            )
                        )
                    }
                )
            );
        }
    }
}