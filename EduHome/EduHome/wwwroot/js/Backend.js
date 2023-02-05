////$(document).ready(function () {


////})
let skipCourse = 6;
let courseCount = $("#loadMore").next().val();
$(document).on("click", "#LoadMore", () => {
  
    $.ajax({

        url: "/Courses/LoadCourses/",
        type: "post",
        data: {
            "skip": skipCourse
        },
        success: function (courses) {

            for (let course of courses) {
                let courseItem =`<div class="col-md-4 col-sm-6 col-xs-12">
                    <div class="single-course mb-70">
                        <div class="course-img">
                            <a href="course-details.html">
                                <img src="/img/course/${course.image}" alt="course">
                                <div class="course-hover">
                                    <i class="fa fa-link"></i>
                                </div>
                            </a>
                        </div>
                        <div class="course-content">
                            <h3><a href="course-details.html">${course.title}</a></h3>
                            <p>${course.description}</p>
                            <a class="default-btn" href="course-details.html">read more</a>
                        </div>
                    </div>
                </div>`
                $("#myCourses").append(courseItem)
            }
            skipCourse += 6;
            console.log(skipCourse)
            console.log(courseCount)


            if (courseCount <= skipCourse) {

                $("#loadMore").remove()
            }
         
        }
      })
   
});